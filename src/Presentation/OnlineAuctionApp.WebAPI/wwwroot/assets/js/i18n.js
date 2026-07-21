const I18n = {
    lang: localStorage.getItem('app_lang') || 'az',

    translations: {
        az: {
            // Navbar
            nav_home: "Ana Səhifə",
            nav_auctions: "Hərraclar",
            nav_login: "Daxil Ol",
            nav_register: "Qeydiyyat",
            nav_seller_panel: "Satıcı Paneli",
            nav_admin_panel: "Admin Paneli",
            nav_categories: "Kategoriyalar",
            nav_logout: "Çıxış Yap",

            // Hero Section
            hero_badge: "Təhlükəsiz & Canlı Hərraclar",
            hero_title: "Nadir və Dəyərli Əşyaların Onlayn Hərrac Mərkəzi",
            hero_desc: "Təhlükəsiz təkliflər verin, kolleksiyanızı genişləndirin və ya öz əşyalarınızı ən yüksək qiymətə satın.",
            hero_btn_auctions: "Hərraclara Bax",
            hero_btn_start: "Satışa Başla",

            // Why Us Section
            why_us_subtitle: "Niyə Bizi Seçməlisiniz?",
            why_us_title: "Ağıllı, Sürətli və Şəffaf Hərrac Təcrübəsi",
            why_us_desc: "BidVerse kolleksiyaçılar və satıcılar üçün ən müasir texnologiyalarla təchiz olunmuş canlı alqı-satqı platformasıdır.",
            
            feature1_title: "Anlıq Və Canlı Təkliflər",
            feature1_desc: "SignalR texnologiyası sayəsində səhifəni yeniləmədən canlı hərrac təkliflərini real vaxtda izləyin və qalib gəlin.",
            
            feature2_title: "100% Təhlükəsiz Sistem",
            feature2_desc: "İstifadəçi hesabları və təkliflər tam qorunur. Hərrac bitənədək ödənişləriniz təhlükəsiz depozitdə saxlanılır.",
            
            feature3_title: "Nadir & Orijinal Əşyalar",
            feature3_desc: "Kolleksiya saatları, retro avtomobillər, sənət əsərləri və unikal elektronika yalnız yoxlanılmış satıcılardan.",

            // CTA Register Banner
            cta_title: "Hələ də hesabınız yoxdur?",
            cta_desc: "İndi qeydiyyatdan keçin, canlı hərraclara ilk təklifinizi verin və ya öz nadir əşyalarınızı dərhal hərraca çıxarın.",
            cta_btn: "İndi Pulsuz Qeydiyyat Ol →",

            // Auctions Section
            active_auctions: "Aktiv Hərraclar",
            active_auctions_sub: "Hazırda canlı təklif qəbul edən hərraclar",
            view_all: "Hamısına bax →",

            // Auctions Page (Filters etc)
            filter_search: "Məhsul axtar...",
            filter_cat: "Bütün Kategoriyalar",
            filter_min: "Min Qiymət",
            filter_max: "Max Qiymət",
            filter_btn: "Filterlə",
            no_auctions: "Axtarışınıza uyğun hərrac tapılmadı.",
            auction_detail: "Detallara Bax",
            current_price: "Cari Qiymət",
            starting_price: "Başlanğıc",
            time_left: "Qalan Vaxt",
            ended: "Bitib",

            // Auction Details
            back_to_auctions: "Hərraclara qayıt",
            category_label: "Kategoriya",
            bid_history: "Təklif Tarixçəsi",
            no_bids_yet: "Hələ heç bir təklif verilməyib.",
            place_bid: "Təklifi Göndər",
            new_bid: "Yeni Təklif Verin (Min:",
            winner: "Qazanmış İstifadəçi:",
            bid_success: "Təklifiniz uğurla qəbul edildi!",
            bid_placeholder: "Təklif məbləği",

            // Auth Pages
            login_title: "BidVerse-ə Giriş",
            login_subtitle: "Davam etmək üçün hesabınıza daxil olun",
            login_username_label: "İstifadəçi adı və ya Email",
            login_password_label: "Şifrə",
            login_btn: "Daxil Ol",
            login_no_account: "Hesabınız yoxdur?",
            login_register_link: "Qeydiyyatdan keçin",
            login_success_redirect: "Giriş edilir...",

            register_title: "Hesab Yaradın",
            register_firstname: "Ad",
            register_lastname: "Soyad",
            register_username: "İstifadəçi Adı",
            register_email: "Email",
            register_role: "Rol Seçimi",
            register_password: "Şifrə",
            register_confirm: "Şifrə Təkrarı",
            register_btn: "Qeydiyyatı Tamamla",
            register_success: "Qeydiyyat uğurla tamamlandı! Giriş edə bilərsiniz.",
            role_buyer: "Alıcı (Buyer)",
            role_seller: "Satıcı (Seller)",

            // Seller Dashboard
            seller_title: "Satıcı Paneli",
            seller_new_auction: "Yeni Hərrac Yarat",
            seller_total: "Ümumi Hərraclar",
            seller_active: "Aktiv Hərraclar",
            seller_completed: "Bitmiş Hərraclar",
            seller_my_auctions: "Mənim Hərraclarım",
            seller_table_product: "Məhsul",
            seller_table_price: "Cari Qiymət",
            seller_table_status: "Status",
            seller_table_end: "Bitiş Vaxtı",
            seller_table_actions: "Əməliyyatlar",
            seller_no_auctions: "Hələ hərracınız yoxdur.",
            seller_upload_image: "Şəkil Yüklə",
            seller_cancel: "Ləğv Et",
            seller_cancel_confirm: "Hərracı ləğv etmək istədiyinizə əminsiniz?",
            seller_create_title: "Yeni Hərrac Yaradın",
            seller_create_btn: "Yarat",
            seller_upload_title: "Şəkil Yüklə",
            seller_upload_btn: "Yüklə",
            seller_cancel_btn: "Ləğv et",
            seller_create_success: "Hərrac uğurla yaradıldı!",
            seller_image_success: "Şəkil yükləndi!",
            seller_cancel_success: "Hərrac ləğv edildi",

            // Create Auction Modal
            create_title_label: "Başlıq",
            create_desc_label: "Təsvir",
            create_price_label: "Başlanğıc Qiyməti",
            create_category_label: "Kategoriya",
            create_endtime_label: "Bitiş Vaxtı",

            // Categories Page
            categories_title: "Kategoriyalar",
            categories_new: "Yeni Kategoriya",
            categories_name: "Adı",
            categories_desc: "Təsviri",
            categories_add: "Əlavə Et",
            categories_existing: "Mövcud Kategoriyalar",
            categories_table_name: "Ad",
            categories_table_desc: "Təsvir",
            categories_table_action: "Əməl",
            categories_delete: "Sil",
            categories_delete_confirm: "Kategoriyanı silməyə əminsiniz?",
            categories_create_success: "Kategoriya yaradıldı!",
            categories_delete_success: "Kategoriya silindi",

            // Notifications Page
            notifications_title: "Bildirişləriniz",
            notifications_empty: "Bildirişiniz yoxdur.",
            notifications_mark_read: "Oxundu İşarələ",
            notifications_read: "Oxunub",

            // Admin Dashboard
            admin_dashboard: "Admin Panel",
            admin_total_auctions: "Ümumi Hərraclar",
            admin_total_users: "Ümumi İstifadəçilər",
            admin_total_bids: "Ümumi Təkliflər",
            admin_total_categories: "Ümumi Kategoriyalar",
            admin_users: "İstifadəçilər",
            admin_auctions: "Hərraclar",
            admin_cancel: "Ləğv Et",
            admin_cancel_confirm: "Admin tərəfindən bu hərracı ləğv etmək istədiyinizə əminsiniz?",
            admin_cancel_success: "Hərrac ləğv olundu",

            // 404/403 Pages
            page_not_found: "Səhifə Tapılmadı",
            page_forbidden: "İcazəniz Yoxdur",
            page_forbidden_desc: "Bu səhifəyə daxil olmaq üçün müvafiq yetkiniz çatmir.",
            back_home: "Ana Səhifəyə Qayıt",

            // Toast Messages
            error_generic: "Xəta baş verdi. Server cavab vermir.",
            error_network: "Şəbəkə xətası baş verdi.",
            error_operation: "Əməliyyat yerinə yetirilə bilmədi.",
            login_error: "Giriş zamanı xəta baş verdi",

            // Common
            signed_in: "Daxil olmusunuz",
            account: "Hesab",
            no_email: "Email mövcud deyil"
        },

        en: {
            // Navbar
            nav_home: "Home",
            nav_auctions: "Auctions",
            nav_login: "Sign In",
            nav_register: "Register",
            nav_seller_panel: "Seller Panel",
            nav_admin_panel: "Admin Panel",
            nav_categories: "Categories",
            nav_logout: "Log Out",

            // Hero Section
            hero_badge: "Safe & Live Auctions",
            hero_title: "Online Auction Hub for Rare & Valuable Items",
            hero_desc: "Place secure bids, expand your collection, or sell your items for the highest price.",
            hero_btn_auctions: "Browse Auctions",
            hero_btn_start: "Start Selling",

            // Why Us Section
            why_us_subtitle: "Why Choose Us?",
            why_us_title: "Smart, Fast and Transparent Auction Experience",
            why_us_desc: "BidVerse is a live trading platform equipped with modern technologies for collectors and sellers.",
            
            feature1_title: "Instant & Live Bids",
            feature1_desc: "Track live auction bids in real-time without refreshing the page thanks to SignalR technology.",
            
            feature2_title: "100% Secure System",
            feature2_desc: "User accounts and bids are fully protected. Payments are safely held in escrow until the auction ends.",
            
            feature3_title: "Rare & Authentic Items",
            feature3_desc: "Collectible watches, retro cars, fine art, and unique electronics strictly from verified sellers.",

            // CTA Register Banner
            cta_title: "Don't have an account yet?",
            cta_desc: "Register now, place your first bid in live auctions, or list your rare items for auction right away.",
            cta_btn: "Register Free Now →",

            // Auctions Section
            active_auctions: "Active Auctions",
            active_auctions_sub: "Auctions currently accepting live bids",
            view_all: "View All →",

            // Auctions Page (Filters etc)
            filter_search: "Search products...",
            filter_cat: "All Categories",
            filter_min: "Min Price",
            filter_max: "Max Price",
            filter_btn: "Filter",
            no_auctions: "No auctions found matching your search.",
            auction_detail: "View Details",
            current_price: "Current Price",
            starting_price: "Starting",
            time_left: "Time Left",
            ended: "Ended",

            // Auction Details
            back_to_auctions: "Back to auctions",
            category_label: "Category",
            bid_history: "Bid History",
            no_bids_yet: "No bids have been placed yet.",
            place_bid: "Place Bid",
            new_bid: "New Bid (Min:",
            winner: "Winner:",
            bid_success: "Your bid was accepted successfully!",
            bid_placeholder: "Bid amount",

            // Auth Pages
            login_title: "Sign In to BidVerse",
            login_subtitle: "Sign in to your account to continue",
            login_username_label: "Username or Email",
            login_password_label: "Password",
            login_btn: "Sign In",
            login_no_account: "Don't have an account?",
            login_register_link: "Register",
            login_success_redirect: "Redirecting...",

            register_title: "Create Account",
            register_firstname: "First Name",
            register_lastname: "Last Name",
            register_username: "Username",
            register_email: "Email",
            register_role: "Select Role",
            register_password: "Password",
            register_confirm: "Confirm Password",
            register_btn: "Complete Registration",
            register_success: "Registration successful! You can now sign in.",
            role_buyer: "Buyer",
            role_seller: "Seller",

            // Seller Dashboard
            seller_title: "Seller Dashboard",
            seller_new_auction: "New Auction",
            seller_total: "Total Auctions",
            seller_active: "Active Auctions",
            seller_completed: "Completed Auctions",
            seller_my_auctions: "My Auctions",
            seller_table_product: "Product",
            seller_table_price: "Current Price",
            seller_table_status: "Status",
            seller_table_end: "End Time",
            seller_table_actions: "Actions",
            seller_no_auctions: "You have no auctions yet.",
            seller_upload_image: "Upload Image",
            seller_cancel: "Cancel",
            seller_cancel_confirm: "Are you sure you want to cancel this auction?",
            seller_create_title: "Create New Auction",
            seller_create_btn: "Create",
            seller_upload_title: "Upload Image",
            seller_upload_btn: "Upload",
            seller_cancel_btn: "Cancel",
            seller_create_success: "Auction created successfully!",
            seller_image_success: "Image uploaded!",
            seller_cancel_success: "Auction cancelled",

            // Create Auction Modal
            create_title_label: "Title",
            create_desc_label: "Description",
            create_price_label: "Starting Price",
            create_category_label: "Category",
            create_endtime_label: "End Time",

            // Categories Page
            categories_title: "Categories",
            categories_new: "New Category",
            categories_name: "Name",
            categories_desc: "Description",
            categories_add: "Add",
            categories_existing: "Existing Categories",
            categories_table_name: "Name",
            categories_table_desc: "Description",
            categories_table_action: "Action",
            categories_delete: "Delete",
            categories_delete_confirm: "Are you sure you want to delete this category?",
            categories_create_success: "Category created!",
            categories_delete_success: "Category deleted",

            // Notifications Page
            notifications_title: "Your Notifications",
            notifications_empty: "You have no notifications.",
            notifications_mark_read: "Mark as Read",
            notifications_read: "Read",

            // Admin Dashboard
            admin_dashboard: "Admin Dashboard",
            admin_total_auctions: "Total Auctions",
            admin_total_users: "Total Users",
            admin_total_bids: "Total Bids",
            admin_total_categories: "Total Categories",
            admin_users: "Users",
            admin_auctions: "Auctions",
            admin_cancel: "Cancel",
            admin_cancel_confirm: "Are you sure you want to cancel this auction as admin?",
            admin_cancel_success: "Auction cancelled",

            // 404/403 Pages
            page_not_found: "Page Not Found",
            page_forbidden: "Access Denied",
            page_forbidden_desc: "You don't have the required permissions to access this page.",
            back_home: "Back to Home",

            // Toast Messages
            error_generic: "An error occurred. Server is not responding.",
            error_network: "A network error occurred.",
            error_operation: "Operation could not be completed.",
            login_error: "An error occurred during login",

            // Common
            signed_in: "Signed in",
            account: "Account",
            no_email: "No email available"
        },

        ru: {
            // Navbar
            nav_home: "Главная",
            nav_auctions: "Аукционы",
            nav_login: "Войти",
            nav_register: "Регистрация",
            nav_seller_panel: "Панель Продавца",
            nav_admin_panel: "Панель Админа",
            nav_categories: "Категории",
            nav_logout: "Выйти",

            // Hero Section
            hero_badge: "Безопасные и Живые Аукционы",
            hero_title: "Онлайн Центр Аукционов Редких и Ценных Вещей",
            hero_desc: "Делайте безопасные ставки, расширяйте коллекцию или продавайте свои вещи по максимальной цене.",
            hero_btn_auctions: "Смотреть Аукционы",
            hero_btn_start: "Начать Продавать",

            // Why Us Section
            why_us_subtitle: "Почему Выбирают Нас?",
            why_us_title: "Умный, Быстрый и Прозрачный Опыт Аукционов",
            why_us_desc: "BidVerse — это платформа торговли в реальном времени, оснащенная передовыми технологиями.",
            
            feature1_title: "Мгновенные и Живые Ставки",
            feature1_desc: "Отслеживайте ставки в режиме реального времени без перезагрузки страницы благодаря SignalR.",
            
            feature2_title: "100% Безопасная Система",
            feature2_desc: "Учетные записи и ставки полностью защищены. Платежи хранятся в безопасности до конца аукциона.",
            
            feature3_title: "Редкие и Подлинные Вещи",
            feature3_desc: "Коллекционные часы, ретро-автомобили, произведения искусства и уникальная электроника от проверенных продавцов.",

            // CTA Register Banner
            cta_title: "У вас еще нет аккаунта?",
            cta_desc: "Зарегистрируйтесь сейчас, сделайте первую ставку или выставите свои редкие вещи на аукцион.",
            cta_btn: "Зарегистрироваться Бесплатно →",

            // Auctions Section
            active_auctions: "Активные Аукционы",
            active_auctions_sub: "Аукционы, принимающие ставки прямо сейчас",
            view_all: "Смотреть все →",

            // Auctions Page (Filters etc)
            filter_search: "Поиск товаров...",
            filter_cat: "Все Категории",
            filter_min: "Мин. Цена",
            filter_max: "Макс. Цена",
            filter_btn: "Фильтр",
            no_auctions: "Аукционы по вашему запросу не найдены.",
            auction_detail: "Подробнее",
            current_price: "Текущая Цена",
            starting_price: "Стартовая",
            time_left: "Осталось Времени",
            ended: "Завершен",

            // Auction Details
            back_to_auctions: "Назад к аукционам",
            category_label: "Категория",
            bid_history: "История Ставок",
            no_bids_yet: "Ставок пока нет.",
            place_bid: "Сделать Ставку",
            new_bid: "Новая Ставка (Мин:",
            winner: "Победитель:",
            bid_success: "Ваша ставка принята!",
            bid_placeholder: "Сумма ставки",

            // Auth Pages
            login_title: "Вход в BidVerse",
            login_subtitle: "Войдите в аккаунт для продолжения",
            login_username_label: "Имя пользователя или Email",
            login_password_label: "Пароль",
            login_btn: "Войти",
            login_no_account: "Нет аккаунта?",
            login_register_link: "Зарегистрироваться",
            login_success_redirect: "Перенаправление...",

            register_title: "Создать Аккаунт",
            register_firstname: "Имя",
            register_lastname: "Фамилия",
            register_username: "Имя пользователя",
            register_email: "Email",
            register_role: "Выберите Роль",
            register_password: "Пароль",
            register_confirm: "Подтвердите Пароль",
            register_btn: "Завершить Регистрацию",
            register_success: "Регистрация успешна! Теперь вы можете войти.",
            role_buyer: "Покупатель",
            role_seller: "Продавец",

            // Seller Dashboard
            seller_title: "Панель Продавца",
            seller_new_auction: "Новый Аукцион",
            seller_total: "Всего Аукционов",
            seller_active: "Активные Аукционы",
            seller_completed: "Завершенные Аукционы",
            seller_my_auctions: "Мои Аукционы",
            seller_table_product: "Товар",
            seller_table_price: "Текущая Цена",
            seller_table_status: "Статус",
            seller_table_end: "Время Окончания",
            seller_table_actions: "Действия",
            seller_no_auctions: "У вас пока нет аукционов.",
            seller_upload_image: "Загрузить Изображение",
            seller_cancel: "Отменить",
            seller_cancel_confirm: "Вы уверены, что хотите отменить этот аукцион?",
            seller_create_title: "Создать Новый Аукцион",
            seller_create_btn: "Создать",
            seller_upload_title: "Загрузить Изображение",
            seller_upload_btn: "Загрузить",
            seller_cancel_btn: "Отмена",
            seller_create_success: "Аукцион успешно создан!",
            seller_image_success: "Изображение загружено!",
            seller_cancel_success: "Аукцион отменен",

            // Create Auction Modal
            create_title_label: "Название",
            create_desc_label: "Описание",
            create_price_label: "Начальная Цена",
            create_category_label: "Категория",
            create_endtime_label: "Время Окончания",

            // Categories Page
            categories_title: "Категории",
            categories_new: "Новая Категория",
            categories_name: "Название",
            categories_desc: "Описание",
            categories_add: "Добавить",
            categories_existing: "Существующие Категории",
            categories_table_name: "Название",
            categories_table_desc: "Описание",
            categories_table_action: "Действие",
            categories_delete: "Удалить",
            categories_delete_confirm: "Вы уверены, что хотите удалить эту категорию?",
            categories_create_success: "Категория создана!",
            categories_delete_success: "Категория удалена",

            // Notifications Page
            notifications_title: "Уведомления",
            notifications_empty: "У вас нет уведомлений.",
            notifications_mark_read: "Отметить как Прочитано",
            notifications_read: "Прочитано",

            // Admin Dashboard
            admin_dashboard: "Панель Администратора",
            admin_total_auctions: "Всего Аукционов",
            admin_total_users: "Всего Пользователей",
            admin_total_bids: "Всего Ставок",
            admin_total_categories: "Всего Категорий",
            admin_users: "Пользователи",
            admin_auctions: "Аукционы",
            admin_cancel: "Отменить",
            admin_cancel_confirm: "Вы уверены, что хотите отменить этот аукцион как администратор?",
            admin_cancel_success: "Аукцион отменен",

            // 404/403 Pages
            page_not_found: "Страница Не Найдена",
            page_forbidden: "Доступ Запрещен",
            page_forbidden_desc: "У вас нет необходимых прав для доступа к этой странице.",
            back_home: "На Главную",

            // Toast Messages
            error_generic: "Произошла ошибка. Сервер не отвечает.",
            error_network: "Произошла сетевая ошибка.",
            error_operation: "Операция не может быть выполнена.",
            login_error: "Ошибка при входе в систему",

            // Common
            signed_in: "Вы вошли",
            account: "Аккаунт",
            no_email: "Email не указан"
        }
    },

    setLang(lang) {
        if (!this.translations[lang]) return;
        this.lang = lang;
        localStorage.setItem('app_lang', lang);
        this.apply();
        
        if (typeof Common !== 'undefined' && Common.renderNavbar) {
            Common.renderNavbar();
        }
    },

    apply() {
        const dictionary = this.translations[this.lang] || this.translations.az;
        
        document.querySelectorAll('[data-i18n]').forEach(element => {
            const key = element.getAttribute('data-i18n');
            if (dictionary[key]) {
                element.innerText = dictionary[key];
            }
        });

        document.querySelectorAll('[data-i18n-placeholder]').forEach(element => {
            const key = element.getAttribute('data-i18n-placeholder');
            if (dictionary[key]) {
                element.placeholder = dictionary[key];
            }
        });
    }
};

document.addEventListener("DOMContentLoaded", () => {
    I18n.apply();
});