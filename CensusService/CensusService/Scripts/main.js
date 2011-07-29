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

$(document).ready(function () {
	initialize();

	$.getJSON('http://jlee/Census/113888907/6224?jsoncallback=?', function (data) {		

		$.each(data, function (i, item) {
			var myLatlng = new google.maps.LatLng(item.GeoLoc.X, item.GeoLoc.Y);
			
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
			new google.maps.Point(0, 35));

			var marker = new google.maps.Marker({
				position: myLatlng,
				map: map,
				title: "Hello World!",
				icon: image,
				shadow: shadow
			});   

			
		})
	});
});