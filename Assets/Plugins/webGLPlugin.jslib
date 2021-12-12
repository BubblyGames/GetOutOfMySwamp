var webGlPlugin = {
    ForceHorizontal: function () {
		var orientation = screen.orientation;
		try{
			console.log("La orientación: " + orientation.type);
			screen.orientation.type = 'landscape-primary';
			//ScreenOrientation.lock("landscape-primary");
			console.log("La orientación nueva: " + screen.orientation.type);
		} catch(e){
				if(e.name !== 'SecurityError'){
				  throw e;
				}
				console.log("Se puede continuar");
				return;
		}
		/*screen.orientation.lock("landscape-primary");
		console.log("La orientación nueva: " + orientation.type);
			if (orientation.type == "portrait"|| orientation.type == "portrait-secondary" || orientation.type == "portrait-primary") {
				screen.orientation = 'landscape-primary';
				screen.orientation.lock('landscape-primary');
			}*/
    },
	
	

    IsMobile: function () {


        return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
    }

};

mergeInto(LibraryManager.library, webGlPlugin);