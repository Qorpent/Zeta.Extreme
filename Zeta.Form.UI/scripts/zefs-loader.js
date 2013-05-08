if((window.useStatic !== undefined) && (window.useStatic == true)) {
	document.write('<script src="scripts/zefs-options.js"></script>');
	document.write('<script src="scripts/zefs-render.js"></script>');
	document.write('<script src="scripts/zefs.js"></script>');
	document.write('<script src="scripts/zefs-forms.js"></script>');
} else {
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-options"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-render"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs"></script>');
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-forms"></script>');
}


document.write('<script src="zeta/getperiods.embedjson.qweb"></script>');
document.write('<script src="zeta/getobjects.embedjson.qweb"></script>');
document.write('<script src="zefs/bizprocesslist.embedjson.qweb"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-debug"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-restart"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-auth"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-authinfo"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-formsave"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-feedback"></script>');
if((window.useStatic !== undefined) && (window.useStatic == true)) {
	document.write('<script src="scripts/zefs-info.js"></script>');
} else {
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-info"></script>');
}
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-hotkeys"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-faq"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-formheader"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-extrapannel"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-table"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-attacher"></script>');
if((window.useStatic !== undefined) && (window.useStatic == true)) {
	document.write('<script src="scripts/zefs-chat.js"></script>');
} else {
	document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-chat"></script>');
}
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-periods"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-objs"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-bizprocess"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-colmanager"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-lockmanager"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-nullrows"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-alerter"></script>');
//document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-report"></script>');
document.write('<script src="_sys/getjs.file.qweb?scriptname=zefs-logo"></script>');
document.write('<script src="scripts/zefs-debug.js"></script>');
document.write('<script src="scripts/zefs-restart.js"></script>');
document.write('<script src="scripts/zefs-auth.js"></script>');
document.write('<script src="scripts/zefs-authinfo.js"></script>');
document.write('<script src="scripts/zefs-formsave.js"></script>');
document.write('<script src="scripts/zefs-feedback.js"></script>');
//document.write('<script src="scripts/zefs-info.js"></script>');
document.write('<script src="scripts/zefs-hotkeys.js"></script>');
document.write('<script src="scripts/zefs-faq.js"></script>');
document.write('<script src="scripts/zefs-formheader.js"></script>');
document.write('<script src="scripts/zefs-extrapannel.js"></script>');
document.write('<script src="scripts/zefs-table.js"></script>');
document.write('<script src="scripts/zefs-attacher.js"></script>');
//document.write('<script src="scripts/zefs-chat.js"></script>');
document.write('<script src="scripts/zefs-periods.js"></script>');
document.write('<script src="scripts/zefs-objs.js"></script>');
document.write('<script src="scripts/zefs-bizprocess.js"></script>');
document.write('<script src="scripts/zefs-colmanager.js"></script>');
document.write('<script src="scripts/zefs-lockmanager.js"></script>');
document.write('<script src="scripts/zefs-nullrows.js"></script>');
document.write('<script src="scripts/zefs-alerter.js"></script>');
document.write('<script src="scripts/zefs-report.js"></script>');
document.write('<script src="scripts/zefs-oldform.js"></script>');