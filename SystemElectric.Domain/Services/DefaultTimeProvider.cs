namespace SystemElectric.TestTask.Domain.Services;

public sealed class DefaultTimeProvider : TimeProvider
{
    private DateTime _dateTime;

    public DefaultTimeProvider()
    {
        Update();
    }

    public override DateTime Now
    {
        get { return _dateTime; }
    }

    public override void Update()
    {
        _dateTime = DateTime.Now;
    }
}
