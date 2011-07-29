var map;

function initialize() {
	var myLatlng = new google.maps.LatLng(37.397, -94);
	var myOptions = {
		zoom: 5,
		center: myLatlng,
		mapTypeId: google.maps.MapTypeId.ROADMAP
	}
	map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
}

function createSmallMarker(i, latitude, longitude) {
	var myLatlng = new google.maps.LatLng(latitude, longitude);

	var image = new google.maps.MarkerImage('http://www.ancestryux.net/dm/censusexplorer/images/results-markers-sm.png',
	// This marker is 20 pixels wide by 32 pixels tall.
			  new google.maps.Size(19.8, 31),
	// The origin for this image is 0,0.
			  new google.maps.Point(19.8 * i, 0),
	// The anchor for this image is the base of the flagpole at 0,32.
			  new google.maps.Point(0, 31));

	var shadow = new google.maps.MarkerImage('http://www.ancestryux.net/dm/censusexplorer/images/marker-sm-shadow.png',
	// The shadow image is larger in the horizontal dimension
	// while the position and offset are the same as for the main image.
			new google.maps.Size(35, 20),
			new google.maps.Point(0, 0),
			new google.maps.Point(-10, 20));

	var marker = new google.maps.Marker({
		position: myLatlng,
		map: map,
		animation: google.maps.Animation.DROP,
		title: "Hello World!",
		icon: image,
		shadow: shadow
	});

	google.maps.event.addListener(marker, 'click', function () {
		if (marker.getAnimation() != null) {
			marker.setAnimation(null);
		} else {
			marker.setAnimation(google.maps.Animation.BOUNCE);
			setTimeout(function () {
				marker.setAnimation(null);
			}, 1500);
		}
	});
}

function createBigMarker(i, latitude, longitude) {
	var myLatlng = new google.maps.LatLng(latitude, longitude);

	var image = new google.maps.MarkerImage('http://www.ancestryux.net/dm/censusexplorer/images/results-markers-lg.png',
	// This marker is 20 pixels wide by 32 pixels tall.
			  new google.maps.Size(30, 50),
	// The origin for this image is 0,0.
			  new google.maps.Point(30 * i, 0),
	// The anchor for this image is the base of the flagpole at 0,32.
			  new google.maps.Point(0, 30));

	var shadow = new google.maps.MarkerImage('http://www.ancestryux.net/dm/censusexplorer/images/marker-lg-shadow.png',
	// The shadow image is larger in the horizontal dimension
	// while the position and offset are the same as for the main image.
			new google.maps.Size(55, 32),
			new google.maps.Point(0, 0),
			new google.maps.Point(-10, 0));

	var marker = new google.maps.Marker({
		position: myLatlng,
		map: map,
		animation: google.maps.Animation.DROP,
		title: "Hello World!",
		icon: image,
		shadow: shadow
	});

	google.maps.event.addListener(marker, 'click', function () {
		if (marker.getAnimation() != null) {
			marker.setAnimation(null);
		} else {
			marker.setAnimation(google.maps.Animation.BOUNCE);
			setTimeout(function() {				
				marker.setAnimation(null);
				}, 1500);
		}
	});
}

$(document).ready(function () {
	initialize();
	/*
	createSmallMarker(0, 34, -82);
	createSmallMarker(1, 37, -122);
	createSmallMarker(2, 36, -110);
	createSmallMarker(3, 35, -80);
	createSmallMarker(4, 38, -90);
	
	
	createBigMarker(0, 34, -82);
	createBigMarker(1, 37, -122);
	createBigMarker(2, 36, -110);
	createBigMarker(3, 35, -80);
	createBigMarker(4, 38, -90);

	*/
	
	
	$.getJSON('http://jlee/Census/113888907/6224?jsoncallback=?', function (data) {

		$.each(data, function (i, item) {
			//createSmallMarker(i, item.GeoLoc.X, item.GeoLoc.Y);
			createBigMarker(i, item.GeoLoc.X, item.GeoLoc.Y);		
		})
	});
	
});