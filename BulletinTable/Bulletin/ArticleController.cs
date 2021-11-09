#pragma warning disable CS8604 // Possible null reference argument.

using BulletinTable.Utils;
using System.Reflection;
using System.Text.Json;

namespace BulletinTable.Bulletin
{
    public class ArticleController
    {
        private readonly List<Article> _articlesList = new List<Article>();
        private readonly Dictionary<string, Article> _articlesDict = new Dictionary<string, Article>();

        public Article? Get(int index)
        {
            if (_articlesList.Count > index)
            {
                LOG.Inst.Error($@"Index out of range! Index:{index} Count of list is: {_articlesList.Count}", MethodBase.GetCurrentMethod());
                return default;
            }

            return _articlesList[index];
        }

        /// <summary>
        /// Adds an article to the collection. 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool Add(Article article)
        {
            if (article == null)
            {
                LOG.Inst.Error($@"The article was null!", MethodBase.GetCurrentMethod());
                return false;
            }

            if (_articlesList.Contains(article))
            {
                LOG.Inst.Error($@"The article already exists in the collection! Article title: {article.GetTitle()}", MethodBase.GetCurrentMethod());
                return false;
            }

            _articlesList.Add(article);
            _articlesDict.TryAdd(article.GetTitle() ?? "NULL", article);

            return true;
        }

        /// <summary>
        /// Adds a new article to the collection using a JSON string.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool Add(string json)
        {
            var article = JsonSerializer.Deserialize<Article>(json);
            var index = _articlesList.Count;

            if (article == null)
            {
                LOG.Inst.Error($@"Not a valid article-json-object!", MethodBase.GetCurrentMethod());
                return false;
            }

            if (_articlesList.Contains(article) || _articlesDict.ContainsKey(article.GetTitle() ?? "NULL" + index))
            {
                LOG.Inst.Error($@"Article '{article.GetTitle()}' is already contained in the collection. Guid:{article.GUID}", MethodBase.GetCurrentMethod());
                return false;
            }
            article.Index = index;

            _articlesList.Add(article);
            var dictSuccess = _articlesDict.TryAdd(article.GetTitle() ?? ("NULL" + index), article);
            var listSuccess = _articlesList.Contains(article);

            if (dictSuccess && listSuccess)
            {
                return true;
            }

            LOG.Inst.Error($@"The article were not added to both collections! Dictionary: {(dictSuccess == true ? "Yes" : "No")} {Environment.NewLine}
                                                                                         List: {(listSuccess == true ? "Yes" : "No")}", MethodBase.GetCurrentMethod());

            return false;
        }

        public EventHandler<Guid>? Added;
        public EventHandler<Guid>? Removed;
        public EventHandler<Guid>? Changed;

    }
}
