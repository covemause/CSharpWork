namespace sample001
{
    public class cmdBase : ICommand
    {
        // データをセット
        public virtual void SetWork(WorkData target) {  }

        // データをカウントアップ
        public virtual void AddCount() { }
    }
}
