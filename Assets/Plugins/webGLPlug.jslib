var MyPlugin = {
     IsMobile: function()
     {
		 console.log("Hola");
         return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
     }
 };
 
 mergeInto(LibraryManager.library, MyPlugin);