namespace SystemElectric.TestTask.Domain.Args
{
    public class TEntityArgs<TEntity> : EventArgs
    {
        public TEntity Entity { get; init; }

        public TEntityArgs(TEntity entity)
        {
            Entity = entity;
        }
    }
}
