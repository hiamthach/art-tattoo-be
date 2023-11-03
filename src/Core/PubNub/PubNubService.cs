namespace art_tattoo_be.Core.PubNub;

using PubnubApi;

public interface IPubNubService
{
  Task Publish(string channel, string message);
}

public class PubNubService : IPubNubService
{
  private readonly Pubnub _pubNub;

  public PubNubService(string userId, string publicKey, string subscribeKey)
  {

    PNConfiguration pnConfiguration = new(new UserId(userId))
    {
      SubscribeKey = subscribeKey,
      PublishKey = publicKey
    };

    Pubnub pubnub = new Pubnub(pnConfiguration);

    _pubNub = pubnub;
  }

  public Task Publish(string channel, string message)
  {
    return _pubNub.Publish()
      .Channel(channel)
      .Message(message)
      .ExecuteAsync();
  }
};
