namespace App.Services;

public static class Endpoints
{
    public static class Feed
    {
        //Recommendations

        /// <summary><b>get</b> The endpoint to retrieve a recommended videos feed.</summary>
        public const string Recommendations = "/feed/recommended";

        /// <summary><b>get</b> The endpoint to retrieve a recommended videos feed.</summary>
        public const string VideoRecommendations = "/feed/recommended?kind=video";

        /// <summary><b>get</b> The endpoint to retrieve a recommended users feed.</summary>
        public const string UserRecommendations = "/feed/recommended?kind=user";


        //Search

        /// <summary><b>get</b> The endpoint to retrieve a searched videos feed.</summary>
        public const string Search = "/feed/search";

        /// <summary><b>get</b> The endpoint to retrieve a searched videos feed.</summary>
        public const string VideoSearch = "/feed/search?kind=video";

        /// <summary><b>get</b> The endpoint to retrieve a searched users feed.</summary>
        public const string UserSearch = "/feed/search?kind=user";


        //Comments

        /// <summary><b>get</b> The endpoint to retrieve video comments. <b>Requires videoid query</b>. <b>Requires id query</b>.</summary>
        public const string Comments = "/feed/comments";
    }

    public static class User
    {
        /// <summary><b>get, (auth) post</b> The endpoint to retrieve user info. <b>Requires id query</b>.</summary>
        public const string Info = "/user/info";

        /// <summary><b>get</b> The endpoint to retrieve user videos. <b>Requires id query</b>.</summary>
        public const string UserVideos = "/user/videos";

        /// <summary><b>(auth) post</b> The endpoint to create user.</summary>
        public const string Create = "/user/create";

        /// <summary><b>(auth) post</b> The endpoint to delete user. <b>Requires id query</b>.</summary>
        public const string Delete = "/user/delete";
    }

    public static class Video
    {
        /// <summary><b>get</b> The endpoint to retrieve video player. <b>Requires id query</b>.</summary>
        public const string Player = "/video/";

        /// <summary><b>get</b> The endpoint to retrieve video user. <b>Requires id query</b>.</summary>
        public const string User = "/video/user";

        /// <summary><b>(auth) put</b> The endpoint to upload / replace video.</summary>
        public const string Upload = "/video/upload";

        /// <summary><b>(auth) post</b> The endpoint to delete video. <b>Requires id query</b>.</summary>
        public const string Delete = "/video/delete";
    }

    public static class Comments
    {
        /// <summary><b>(auth) put</b> The endpoint to add or update a comment for a video. <b>Requires videoid query</b>. <b>Requires model</b>.</summary>
        public const string PostComment = "/comments";

        /// <summary><b>(auth) post</b> The endpoint to delete a comment for a video. <b>Requires videoid query</b>. <b>Requires id query</b>.</summary>
        public const string DeleteComment = "/comments/delete";
    }
}
