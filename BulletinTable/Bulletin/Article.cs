using BulletinTable.Storage;
using BulletinTable.Utils;
using System.Reflection;

namespace BulletinTable.Bulletin
{
    public class Article
    {
        public Guid GUID { get; internal set; }
        public int Index { get; internal set; }

        public DateTime CreatedDate { get; internal set; }
        public DateTime UpdatedDate { get; internal set; }
        public Guid Creator { get; internal set; }
        public string? CreatorName { get; internal set; }

        private string? title;
        private string? content;

        public string? GetTitle() => title;
        public string? GetContent() => content;

        public void SetTitle(string? value)
        {
            if (string.IsNullOrEmpty(value)) return;
            title = value;
            TitleChanged?.Invoke(this, title);
            OnAnyChanged();
        }

        public void SetContent(string? value)
        {
            if (string.IsNullOrEmpty(value)) return;
            content = value;
            ContentChanged?.Invoke(this, EventArgs.Empty);
            OnAnyChanged();
        }

        private void OnAnyChanged()
        {
            UpdatedDate = DateTime.Now;
        }


        public EventHandler? ContentChanged;
        public EventHandler<string>? TitleChanged;

        public Article(string? title)
        {
            if (title == null)
            {
                LOG.Inst.Error($@"Title was null", MethodBase.GetCurrentMethod());
                return;
            }

            this.title = title;
        }
    }
}
