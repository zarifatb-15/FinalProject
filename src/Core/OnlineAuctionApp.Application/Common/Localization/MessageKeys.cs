namespace OnlineAuctionApp.Application.Common.Localization;

public static class MessageKeys
{
    public const string AuthRequired = "Auth.Required";
    public const string InvalidToken = "Auth.InvalidToken";
    public const string Forbidden = "Auth.Forbidden";
    public const string InvalidUserToken = "Auth.InvalidUserToken";

    public const string AuctionNotFound = "Auction.NotFound";
    public const string AuctionNotActive = "Auction.NotActive";
    public const string AuctionAlreadyEnded = "Auction.AlreadyEnded";
    public const string AuctionHasNotEndedYet = "Auction.HasNotEndedYet";

    public const string BidAmountGreaterThanZero = "Bid.AmountGreaterThanZero";
    public const string BidAmountGreaterThanCurrentPrice = "Bid.AmountGreaterThanCurrentPrice";
    public const string SellerCannotBidOwnAuction = "Bid.SellerCannotBidOwnAuction";

    public const string OutbidNotification = "Notification.Outbid";
    public const string SellerAuctionEndedNoBids = "Notification.SellerAuctionEndedNoBids";
    public const string SellerAuctionEndedWithWinner = "Notification.SellerAuctionEndedWithWinner";
    public const string BuyerWonAuction = "Notification.BuyerWonAuction";
    public const string BuyerLostAuction = "Notification.BuyerLostAuction";
}