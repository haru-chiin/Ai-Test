// MicrophonePlugin.jslib
mergeInto(LibraryManager.library, {
    initMicrophone: function() {
        navigator.mediaDevices.getUserMedia({ audio: true })
            .then(function(stream) {
                console.log("Microphone access granted");
                // Lakukan sesuatu dengan stream audio di sini
            })
            .catch(function(err) {
                console.error("Error accessing microphone: " + err);
            });
    }
});