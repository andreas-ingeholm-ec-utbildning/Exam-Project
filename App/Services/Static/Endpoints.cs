namespace App.Services;

public static class Endpoints
{
    public static class Feed
    {
        //Recommendations

        /// <summary>The endpoint to retrieve a recommended videos feed.</summary>
        /// <remarks><b>get</b></remarks>
        public const string Recommendations = "/feed/recommended";

        /// <summary>The endpoint to retrieve a recommended videos feed.</summary>
        /// <remarks><b>get</b></remarks>
        public const string VideoRecommendations = "/feed/recommended?kind=video";

        /// <summary>The endpoint to retrieve a recommended users feed.</summary>
        /// <remarks><b>get</b></remarks>
        public const string UserRecommendations = "/feed/recommended?kind=user";


        //Search

        /// <summary>The endpoint to retrieve a searched videos feed.</summary>
        /// <remarks><b>get</b></remarks>
        public const string Search = "/feed/search";

        /// <summary>The endpoint to retrieve a searched videos feed.</summary>
        /// <remarks><b>get</b></remarks>
        public const string VideoSearch = "/feed/search?kind=video";

        /// <summary>The endpoint to retrieve a searched users feed.</summary>
        /// <remarks><b>get</b></remarks>
        public const string UserSearch = "/feed/search?kind=user";


        //Comments

        /// <summary>The endpoint to retrieve video comments. <b>Requires videoid query</b>. <b>Requires id query</b>.</summary>
        /// <remarks><b>get</b></remarks>
        public const string Comments = "/feed/comments";
    }

    public static class User
    {
        /// <summary>The endpoint to retreive my user view.</summary>
        /// <remarks><b>get</b></remarks>
        public const string Me = "/user/me";

        public const string Login = "/user/login";
        public const string Logout = "/user/logout";
        public const string Edit = "/user/edit";
        public const string Create = "/user/create";
        public const string Bookmarks = "/user/bookmarks";
        public const string Upload = "/user/upload";

        /// <summary>The endpoint to retrieve user info. <b>Requires id query</b>.</summary>
        /// <remarks><b>get, (auth) post</b></remarks>
        public const string Info = "/user/info";

        /// <summary>The endpoint to retrieve user videos. <b>Requires id query</b>.</summary>
        /// <remarks><b>get</b></remarks>
        public const string UserVideos = "/user/videos";
        public const string UserComments = "/user/comments";

        /// <summary>The endpoint to delete user. <b>Requires id query</b>.</summary>
        /// <remarks><b>(auth) post</b></remarks>
        public const string Delete = "/user/delete";
    }

    public static class Video
    {
        /// <summary>The endpoint to retrieve video player. <b>Requires id query</b>.</summary>
        /// <remarks><b>get</b></remarks>
        public const string Player = "/video/";

        /// <summary>The endpoint to retrieve video user. <b>Requires id query</b>.</summary>
        /// <remarks><b>get</b></remarks>
        public const string User = "/video/user";

        /// <summary>The endpoint to upload / replace video.</summary>
        /// <remarks><b>(auth) put</b></remarks>
        public const string Upload = "/video/upload";

        /// <summary>The endpoint to delete video. <b>Requires id query</b>.</summary>
        /// <remarks><b>(auth) post</b></remarks>
        public const string Delete = "/video/delete";
    }

    public static class Comments
    {
        /// <summary>The endpoint to add or update a comment for a video. <b>Requires videoid query</b>. <b>Requires model</b>.</summary>
        /// <remarks><b>(auth) put</b></remarks>
        public const string PostComment = "/comments";

        /// <summary>The endpoint to delete a comment for a video. <b>Requires videoid query</b>. <b>Requires id query</b>.</summary>
        /// <remarks><b>(auth) post</b></remarks>
        public const string DeleteComment = "/comments/delete";
    }
}
