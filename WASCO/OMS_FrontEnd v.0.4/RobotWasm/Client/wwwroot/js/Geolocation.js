function getLocation() {
    if (navigator.geolocation) {
        // Geolocation is supported
        navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
    } else {
        // Geolocation is not supported
        console.log("Geolocation is not supported by this browser.");
    }
}

function successCallback(position) {
    // The user has granted permission and the browser has retrieved the location
    var latitude = position.coords.latitude;
    var longitude = position.coords.longitude;

    console.log("Latitude: " + latitude);
    console.log("Longitude: " + longitude);
}

function errorCallback(error) {
    // An error occurred while retrieving location or the user denied permission
    switch (error.code) {
        case error.PERMISSION_DENIED:
            console.log("User denied the request for Geolocation.");
            break;
        case error.POSITION_UNAVAILABLE:
            console.log("Location information is unavailable.");
            break;
        case error.TIMEOUT:
            console.log("The request to get user location timed out.");
            break;
        case error.UNKNOWN_ERROR:
            console.log("An unknown error occurred.");
            break;
    }
}
