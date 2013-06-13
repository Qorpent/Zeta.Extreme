(function($) {
    qwiki.getCustomReference = function(processor,addr,name,tail){
        // reference to another wiki page
        if(addr.match(/^\//)){
            return "<span class='wiki-link' code='"+addr+"'>"+name+"</span>";
        }
        //reference to wiki-image
        else if(addr.match(/^img:/)){
            var imgcode = addr.match(/^img:([\s\S]+)$/)[1];
            var imgaddr = "./wiki/getfile.filedesc.qweb?code="+imgcode;
			if(tail.match(/pict/)){
            return "<div  class='wiki-img'><div class='wiki-img-title'>"+name+"</div><img src='"+imgaddr+"' title='"+name+"' "+tail+" /></div>";
			}else{
				return "<img src='"+imgaddr+"' title='"+name+"' "+tail+" />";
			}
        }
        //reference to wiki-filedesc
        else if (addr.match(/^file:/)){
            var filecode = addr.match(/^file:([\s\S]+)$/)[1];
            var fileaddr = "./wiki/getfile.filedesc.qweb?asfile=true&code="+filecode;
            return "<a href='"+fileaddr+"' "+tail+" target='_blank' >"+name+"</a>";
        }
        // not custom
        return null;
    };
    $(document).delegate(".wiki-link", "click", function(e) {
        e.stopPropagation();
        zefs.api.wiki.getsync.execute({code: $(this).attr("code")});
    });
})(jQuery);