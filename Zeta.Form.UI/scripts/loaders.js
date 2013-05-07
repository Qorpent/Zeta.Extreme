if((window.useStatic !== undefined) && (window.useStatic == true)) {
    document.write('<script src="scripts/qweb.js" type="text/javascript"></script>');
 	document.write('<script src="scripts/zeta-loader.js"></script>');
	document.write('<script src="scripts/zefs-loader.js"></script>');
} else {
	document.write('<script src="_sys/getjs.file.qweb?scriptname=qweb" type="text/javascript"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta-loader"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-loader"></script>');
}