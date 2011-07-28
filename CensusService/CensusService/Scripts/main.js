$(document).ready(function () {
	$.getJSON('http://jlee/census/113888907/6224?jsoncallback=?', function (data) {
		console.log(data);
	});
});