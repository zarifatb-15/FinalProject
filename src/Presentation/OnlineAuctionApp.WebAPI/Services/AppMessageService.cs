using OnlineAuctionApp.Application.Common.Localization;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Services;

public class AppMessageService : IAppMessageService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppMessageService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private static readonly Dictionary<string, Dictionary<string, string>> Messages = new()
    {
        ["az"] = new Dictionary<string, string>
        {
            [MessageKeys.AuthRequired] = "Daxil olmaq tələb olunur.",
            [MessageKeys.InvalidToken] = "Sessiya bitib və ya token yanlışdır.",
            [MessageKeys.Forbidden] = "Bu resursa giriş icazəniz yoxdur.",
            [MessageKeys.InvalidUserToken] = "İstifadəçi token-i yanlışdır.",

            [MessageKeys.AuctionNotFound] = "Hərrac tapılmadı.",
            [MessageKeys.AuctionNotActive] = "Hərrac aktiv deyil.",
            [MessageKeys.AuctionAlreadyEnded] = "Hərrac artıq başa çatıb.",
            [MessageKeys.AuctionHasNotEndedYet] = "Hərracın bitmə vaxtı hələ çatmayıb.",

            [MessageKeys.BidAmountGreaterThanZero] = "Təklif məbləği sıfırdan böyük olmalıdır.",
            [MessageKeys.BidAmountGreaterThanCurrentPrice] = "Təklif məbləği cari qiymətdən yüksək olmalıdır.",
            [MessageKeys.SellerCannotBidOwnAuction] = "Seller öz hərracına təklif verə bilməz.",

            [MessageKeys.OutbidNotification] = "'{0}' hərracında sizi keçən yeni təklif verildi.",
            [MessageKeys.SellerAuctionEndedNoBids] = "'{0}' hərracınız heç bir təklif olmadan başa çatdı.",
            [MessageKeys.SellerAuctionEndedWithWinner] = "'{0}' hərracınız başa çatdı. Qalib təklif: {1}.",
            [MessageKeys.BuyerWonAuction] = "Təbriklər! '{0}' hərracının qalibi siz oldunuz.",
            [MessageKeys.BuyerLostAuction] = "'{0}' hərracı başa çatdı. Bu hərracda qalib olmadınız."
        },

        ["en"] = new Dictionary<string, string>
        {
            [MessageKeys.AuthRequired] = "Authentication is required.",
            [MessageKeys.InvalidToken] = "Invalid or expired authentication token.",
            [MessageKeys.Forbidden] = "You are not allowed to access this resource.",
            [MessageKeys.InvalidUserToken] = "Invalid user token.",

            [MessageKeys.AuctionNotFound] = "Auction not found.",
            [MessageKeys.AuctionNotActive] = "Auction is not active.",
            [MessageKeys.AuctionAlreadyEnded] = "Auction has already ended.",
            [MessageKeys.AuctionHasNotEndedYet] = "Auction has not ended yet.",

            [MessageKeys.BidAmountGreaterThanZero] = "Bid amount must be greater than zero.",
            [MessageKeys.BidAmountGreaterThanCurrentPrice] = "Bid amount must be greater than current price.",
            [MessageKeys.SellerCannotBidOwnAuction] = "Seller cannot bid on their own auction.",

            [MessageKeys.OutbidNotification] = "You have been outbid on auction '{0}'.",
            [MessageKeys.SellerAuctionEndedNoBids] = "Your auction '{0}' has ended without any bids.",
            [MessageKeys.SellerAuctionEndedWithWinner] = "Your auction '{0}' has ended. Winning bid: {1}.",
            [MessageKeys.BuyerWonAuction] = "Congratulations! You won auction '{0}'.",
            [MessageKeys.BuyerLostAuction] = "Auction '{0}' has ended. You did not win this auction."
        },

        ["ru"] = new Dictionary<string, string>
        {
            [MessageKeys.AuthRequired] = "Требуется вход в систему.",
            [MessageKeys.InvalidToken] = "Сессия истекла или токен недействителен.",
            [MessageKeys.Forbidden] = "У вас нет доступа к этому ресурсу.",
            [MessageKeys.InvalidUserToken] = "Токен пользователя недействителен.",

            [MessageKeys.AuctionNotFound] = "Аукцион не найден.",
            [MessageKeys.AuctionNotActive] = "Аукцион не активен.",
            [MessageKeys.AuctionAlreadyEnded] = "Аукцион уже завершён.",
            [MessageKeys.AuctionHasNotEndedYet] = "Время окончания аукциона ещё не наступило.",

            [MessageKeys.BidAmountGreaterThanZero] = "Сумма ставки должна быть больше нуля.",
            [MessageKeys.BidAmountGreaterThanCurrentPrice] = "Сумма ставки должна быть выше текущей цены.",
            [MessageKeys.SellerCannotBidOwnAuction] = "Seller не может делать ставку на свой аукцион.",

            [MessageKeys.OutbidNotification] = "В аукционе '{0}' была сделана ставка выше вашей.",
            [MessageKeys.SellerAuctionEndedNoBids] = "Ваш аукцион '{0}' завершён без ставок.",
            [MessageKeys.SellerAuctionEndedWithWinner] = "Ваш аукцион '{0}' завершён. Победившая ставка: {1}.",
            [MessageKeys.BuyerWonAuction] = "Поздравляем! Вы выиграли аукцион '{0}'.",
            [MessageKeys.BuyerLostAuction] = "Аукцион '{0}' завершён. Вы не выиграли этот аукцион."
        }
    };

    public string Get(string key)
    {
        var language = GetCurrentLanguage();

        if (Messages.TryGetValue(language, out var selectedMessages) &&
            selectedMessages.TryGetValue(key, out var message))
        {
            return message;
        }

        return Messages["az"].TryGetValue(key, out var defaultMessage)
            ? defaultMessage
            : key;
    }

    public string Get(string key, params object[] args)
    {
        var message = Get(key);
        return string.Format(message, args);
    }

    private string GetCurrentLanguage()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var languageFromHeader = request?.Headers["Accept-Language"].ToString();

        if (string.IsNullOrWhiteSpace(languageFromHeader))
        {
            return "az";
        }

        if (languageFromHeader.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
        {
            return "ru";
        }

        if (languageFromHeader.StartsWith("en", StringComparison.OrdinalIgnoreCase))
        {
            return "en";
        }

        return "az";
    }
}