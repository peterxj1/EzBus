namespace EzBus
{
    public interface IPublishingChannel
    {
        void Publish(ChannelMessage cm);
    }
}