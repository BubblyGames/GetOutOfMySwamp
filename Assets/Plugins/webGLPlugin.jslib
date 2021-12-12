var webGlPlugin = {
    ForceHorizontal: function () {
        var orientation = screen.orientation;
        if (orientation === "portrait"|| orientation === "portrait-secondary" || orientation === "portrait-primary") {
            screen.orientation = 'landscape-primary';
            screen.orientation.lock('landscape-primary');
        }
    },
	
	// Custom check function
	function customCheck(unityInstance, onsuccess, onerror) {
		console.log("Ejecuté customCheck");
    // Check if the browser hasn't WebGL
		if (!UnityLoader.SystemInfo.hasWebGL) {
			unityInstance.popup("Your browser does not support WebGL.", [{ text: "Stop", callback: onerror }]);
		}
    // Check if mobile platform
		else if (UnityLoader.SystemInfo.mobile) {
			unityInstance.popup("Unity WebGL is not supported on mobile, but you can continue anyway.", [
				{ text: "Stop", callback: onerror },
				{ text: "Continue", callback: onsuccess },
			]);
		}
		// Check if not supported browser
		else if (["Edge", "Firefox", "Chrome", "Safari"].indexOf(UnityLoader.SystemInfo.browser) == -1) {
			unityInstance.popup("Your browser is not supported, but you can continue anyway.", [
				{ text: "Stop", callback: onerror },
				{ text: "Continue", callback: onsuccess },
			]);
		} else {
			onsuccess();
		}
}

    IsMobile: function () {


        return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
    }

};

mergeInto(LibraryManager.library, webGlPlugin);