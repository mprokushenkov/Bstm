using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Bstm.Common;
using JetBrains.Annotations;
using NLog;

namespace Bstm.DirectoryServices
{
    public class DirectoryProvider
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public virtual T GetDirectoryObject<T>(AdsPath adsPath)
            where T : class, IDirectoryObject
        {
            return GetDirectoryObject(adsPath) as T;
        }

        public virtual IDirectoryObject GetDirectoryObject(AdsPath adsPath)
        {
            var directoryObject = new DirectoryEntry(adsPath).ToDirectoryObject();

            return directoryObject;
        }

        public virtual T FindOne<T>(
            [NotNull] AdsPath searchRoot,
            [NotNull] DirectoryProperty directoryProperty,
            object propertyValue)
            where T : class, IDirectoryObject
        {
            return FindOne(searchRoot, directoryProperty, propertyValue) as T;
        }

        public virtual IDirectoryObject FindOne(
            [NotNull] AdsPath searchRoot,
            [NotNull] DirectoryProperty directoryProperty,
            object propertyValue)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            var searchFilter = SearchFilter.Equality(
                directoryProperty,
                directoryProperty.CreateSearchFilterString(propertyValue));

            return FindOne(searchRoot, searchFilter);
        }

        public virtual T FindOne<T>([NotNull] AdsPath searchRoot, [NotNull] SearchFilter searchFilter)
            where T : class, IDirectoryObject
        {
            return FindOne(searchRoot, searchFilter) as T;
        }

        public virtual IDirectoryObject FindOne([NotNull] AdsPath searchRoot, [NotNull] SearchFilter searchFilter)
        {
            Guard.CheckNull(searchRoot, nameof(searchRoot));
            Guard.CheckNull(searchFilter, nameof(searchFilter));

            using (var root = new DirectoryEntry(searchRoot))
            {
                logger.Debug("Search one object in {SearchRoot} by filter {SearchFilter}.", searchRoot, searchFilter);

                var searcher = CreateDirectorySearcher(searchFilter, root);
                var searchResult = searcher.FindOne();

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse - Reason: Incorrect analysis
                // ReSharper disable HeuristicUnreachableCode - Reason: Incorrect analysis
                if (searchResult == null)
                {
                    logger.Debug("Search has no result.");
                    return null;
                }
                // ReSharper restore HeuristicUnreachableCode

                var directoryEntry = searchResult.GetDirectoryEntry();

                var directoryObject = directoryEntry.ToDirectoryObject();

                logger.Debug("{DirectoryObject} has been found.", directoryObject);

                return directoryObject;
            }
        }

        public virtual IEnumerable<IDirectoryObject> FindAll(
            [NotNull] AdsPath searchRootAdsPath,
            [NotNull] SearchFilter searchFilter)
        {
            Guard.CheckNull(searchRootAdsPath, nameof(searchRootAdsPath));
            Guard.CheckNull(searchFilter, nameof(searchFilter));

            logger.Debug("Search multiple objects in {SearchRoot} by filter {SearchFilter}.", searchRootAdsPath, searchFilter);

            using (var root = new DirectoryEntry(searchRootAdsPath))
            {
                var searcher = CreateDirectorySearcher(searchFilter, root);
                var results = FindAllInternal(searcher);
                return results;
            }
        }

        public virtual IEnumerable<IDirectoryObject> FindAll(
            [NotNull] AdsPath searchRootAdsPath,
            [NotNull] SearchFilter searchFilter,
            [NotNull] DirectoryProperty asqProperty)
        {
            Guard.CheckNull(searchRootAdsPath, nameof(searchRootAdsPath));
            Guard.CheckNull(searchFilter, nameof(searchFilter));
            Guard.CheckNull(asqProperty, nameof(asqProperty));

            if (asqProperty.Syntax != DirectoryPropertySyntax.DNString)
            {
                throw new DirectoryServicesException(ErrorMessages.DirectoryServicesException_InvalidAsqPropertySyntax);
            }

            logger.Debug(
                "Search multiple objects in {SearchRoot} by {AsqProperty} with filter {SearchFilter}.",
                searchRootAdsPath,
                asqProperty,
                searchFilter);

            using (var root = new DirectoryEntry(searchRootAdsPath))
            {
                var searcher = CreateDirectorySearcher(searchFilter, root);
                searcher.SearchScope = SearchScope.Base;
                searcher.AttributeScopeQuery = asqProperty;
                searcher.ReferralChasing = ReferralChasingOption.Subordinate;
                searcher.PropertyNamesOnly = true;

                var results = FindAllInternal(searcher);
                return results;
            }
        }

        private IEnumerable<IDirectoryObject> FindAllInternal(DirectorySearcher searcher)
        {
            var searchResult = searcher.FindAll();

            var results = searchResult
                .Cast<SearchResult>()
                .Select(r => r.GetDirectoryEntry())
                .Select(e => e.ToDirectoryObject());

            return results;
        }

        private static DirectorySearcher CreateDirectorySearcher(SearchFilter searchFilter, DirectoryEntry root)
        {
            var searcher = new DirectorySearcher(root);
            searcher.Filter = searchFilter;
            return searcher;
        }
    }
}