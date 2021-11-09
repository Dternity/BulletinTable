namespace BulletinTable.Bulletin
{
    public class RootController
    {
        public static readonly RootController _inst = new RootController();

        public ArticleController? ArticleController { get; private set; }

        
        public void Init()
        {
            _inst.ArticleController = new ArticleController();

            var article = new Article { Title = "ArticleTest!", Content = "This true content!", GUID = Guid.NewGuid() };
            _inst.ArticleController.Add(article);

        }
    }
}
