var webGlPlugin = {
     ForceHorizontal: function () {
        var orientation = screen.orientation;
        if (orientation === "portrait"|| orientation === "portrait-secondary" || orientation === "portrait-primary") {
            screen.orientation = "landscape-primary";
            screen.orientation.lock("landscape-primary");
        }
	 },	

    IsMobile: function () {


        return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
    }

};

mergeInto(LibraryManager.library, webGlPlugin);