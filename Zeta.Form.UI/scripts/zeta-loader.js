if((window.useStatic !== undefined) && (window.useStatic == true)) {
	document.write('<script src="scripts/zeta-options.js" type="text/javascript"></script>');
	document.write('<script src="scripts/zeta.js" type="text/javascript"></script>');
	document.write('<script src="scripts/zeta-modal.js" type="text/javascript"></script>');
	document.write('<script src="scripts/zeta-storage.js" type="text/javascript"></script>');
	document.write('<script src="scripts/zeta-user.js" type="text/javascript"></script>') ;
	document.write('<script src="scripts/zeta-floatmenu.js" type="text/javascript"></script>');
	document.write('<link rel="stylesheet" type="text/css" href="styles/zeta.css">');
} else {
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta-options" type="text/javascript"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta" type="text/javascript"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta-modal" type="text/javascript"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta-storage" type="text/javascript"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta-user" type="text/javascript"></script>') ;
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zeta-floatmenu" type="text/javascript"></script>');
	document.write('<link rel="stylesheet" type="text/css" href="_sys/getcss.file.qweb?stylename=zeta">');
}