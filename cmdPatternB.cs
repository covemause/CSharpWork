namespace sample001
{
    /// <summary>
    /// 奇数を処理するコマンド
    /// </summary>
    class cmdPatternB : cmdBase
    {
        protected WorkData wd;

        public override void SetWork(WorkData target)
        {
            this.wd = target;
        }

        public override void AddCount()
        {
            this.wd.odd += 1;
        }

    }
}
