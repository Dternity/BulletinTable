using BulletinTable.Utils;
using System.Linq;
using System.Reflection;

namespace BulletinTable.Bulletin
{
    public class ArticleController
    {
        private List<Article> _articlesList = new List<Article>();
        private Dictionary<string, Article> _articlesDict = new Dictionary<string, Article>();

        public Article? Get(int index)
        {
            if (_articlesList.Count > index)
            {
                LOG.Inst.Error($@"Index out of range! Index:{index} Count of list is: {_articlesList.Count}", MethodBase.GetCurrentMethod());
                return default;
            }

            return _articlesList[index];
        }

        public bool Add(Article article)
        {
            if(article == null)
            {
                LOG.Inst.Error($@"The article was null!", MethodBase.GetCurrentMethod());
                return false;
            }

            if (_articlesList.Contains(article))
            {
                LOG.Inst.Error($@"The article already exists in the collection! Article title: {article.GetTitle()}", MethodBase.GetCurrentMethod());
                return false;
            }
            var title = article.GetTitle() ?? "NULL";

            _articlesList.Add(article);
            _articlesDict.TryAdd(title, article);

            return true;
        }

        public EventHandler<Guid>? Added;
        public EventHandler<Guid>? Removed;
        public EventHandler<Guid>? Changed;

    }
}
