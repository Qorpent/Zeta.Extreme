window.useStatic = true;
var siteroot = document.location.pathname.match("^/([\\w\\d_\-]+)?/")[0];

function isStatic() {
	if(
		(window.useStatic !== undefined) && (window.useStatic == true)
	) {
		return true;
	} else {
		return false;
	}
}