var root = window.zeta = window.zeta || {};
var jsformat = root.jsformat = root.jsformat || {};
(function () {
    "use strict";

    $.extend(jsformat,{
        TYPE_STRING : 1,
        TYPE_NUMBER : 2,
        TYPE_OBJECT : 3,
        TYPE_ARRAY  : 4,
        TYPE_BOOL   : 5,
        TYPE_NULL   : 6,

        baseSpan : document.createElement('span'),

        getSpanBoth : function(innerText,className) {
            var span = this.baseSpan.cloneNode(false);
            span.className = className;
            span.innerText = innerText;
            return span;
        },

        getSpanClass : function(className) {
            var span = this.baseSpan.cloneNode(false);
            span.className = className;
            return span;
        },

        getKvovDOM : function(value, keyName) {
            var type,
                kvov,
                nonZeroSize,
                templates = this.templatesObj,
                objKey,
                keySpan,
                valueElement
              ;
            if (typeof value === 'string')
                type = this.TYPE_STRING;
            else if (typeof value === 'number')
                type = this.TYPE_NUMBER;
            else if (value === false || value === true)
                type = this.TYPE_BOOL;
            else if (value === null)
                type = this.TYPE_NULL;
            else if (value instanceof Array)
                type = this.TYPE_ARRAY;
            else
                type = this.TYPE_OBJECT;
            kvov = templates.t_kvov.cloneNode(false);
            if (type === this.TYPE_OBJECT || type === this.TYPE_ARRAY) {
                nonZeroSize = false;
                for (objKey in value) {
                    if (value.hasOwnProperty(objKey)) {
                        nonZeroSize = true;
                        break;
                    }
                }
                if (nonZeroSize)
                    kvov.appendChild(  templates.t_exp.cloneNode(false));
            }
            if (keyName !== false) {
                kvov.classList.add('objProp');
                keySpan = templates.t_key.cloneNode(false);
                keySpan.textContent = JSON.stringify(keyName).slice(1,-1);
                kvov.appendChild(templates.t_dblqText.cloneNode(false));
                kvov.appendChild( keySpan);
                kvov.appendChild(templates.t_dblqText.cloneNode(false));
                kvov.appendChild( templates.t_colonAndSpace.cloneNode(false));
            }
            else {
                kvov.classList.add('arrElem');
            }
            var blockInner, childKvov;
            switch (type) {
                case this.TYPE_STRING :
                    var innerStringEl = this.baseSpan.cloneNode(false),
                        escapedString = JSON.stringify(value)
                      ;
                    escapedString = escapedString.substring(1, escapedString.length-1);
                    if (value[0] === 'h' && value.substring(0, 4) === 'http') {
                        var innerStringA = document.createElement('A');
                        innerStringA.href = value;
                        innerStringA.innerText = escapedString;
                        innerStringEl.appendChild(innerStringA);
                    }
                    else {
                        innerStringEl.innerText = escapedString;
                    }
                    valueElement = templates.t_string.cloneNode(false);
                    valueElement.appendChild(templates.t_dblqText.cloneNode(false));
                    valueElement.appendChild(innerStringEl);
                    valueElement.appendChild(templates.t_dblqText.cloneNode(false));
                    kvov.appendChild(valueElement);
                    break;

                case this.TYPE_NUMBER:
                    valueElement = templates.t_number.cloneNode(false);
                    valueElement.innerText = value;
                    kvov.appendChild(valueElement);
                    break;

                case this.TYPE_OBJECT:
                    kvov.appendChild( templates.t_oBrace.cloneNode(true));
                    if (nonZeroSize) {
                        kvov.appendChild( templates.t_ellipsis.cloneNode(false));
                        blockInner = templates.t_blockInner.cloneNode(false);
                        var count = 0, k, comma;
                        for (k in value) {
                            if (value.hasOwnProperty(k)) {
                                count++;
                                childKvov = this.getKvovDOM(value[k], k);
                                comma = templates.t_commaText.cloneNode();
                                childKvov.appendChild(comma);
                                blockInner.appendChild( childKvov);
                            }
                        }
                        childKvov.removeChild(comma);
                        kvov.appendChild( blockInner);
                    }
                    kvov.appendChild( templates.t_cBrace.cloneNode(true));
                    break;

                case this.TYPE_ARRAY:
                    kvov.appendChild( templates.t_oBracket.cloneNode(true));
                    if (nonZeroSize) {
                        kvov.appendChild( templates.t_ellipsis.cloneNode(false));
                        blockInner = templates.t_blockInner.cloneNode(false);
                        for (var i=0, length=value.length, lastIndex=length-1; i<length; i++) {
                            childKvov = this.getKvovDOM(value[i], false);
                            if (i < lastIndex)
                                childKvov.appendChild( templates.t_commaText.cloneNode());
                            blockInner.appendChild( childKvov);
                        }
                        kvov.appendChild( blockInner);
                    }
                    kvov.appendChild( templates.t_cBracket.cloneNode(true));
                    break;

                case this.TYPE_BOOL:
                    if (value)
                        kvov.appendChild( templates.t_true.cloneNode(true));
                    else
                        kvov.appendChild( templates.t_false.cloneNode(true));
                    break;

                case this.TYPE_NULL:
                    kvov.appendChild( templates.t_null.cloneNode(true));
                    break;
            }
            return kvov;
        }
    });

    jsformat.lastKvovIdGiven = 0;

    jsformat.collapse = function(elements) {
        var el, i, blockInner, count;
        for (i = elements.length - 1; i >= 0; i--) {
            el = elements[i];
            el.classList.add('collapsed');
            if (!el.id) {
                el.id = 'kvov' + (++jsformat.lastKvovIdGiven);
                blockInner = el.firstElementChild;
                while ( blockInner && !blockInner.classList.contains('blockInner')) {
                    blockInner = blockInner.nextElementSibling;
                }
                if (!blockInner) continue;
                count = blockInner.children.length;
            }
        }
    };

    jsformat.expand = function(elements) {
        for (var i = elements.length - 1; i >= 0; i--)
            elements[i].classList.remove('collapsed');
    };


    jsformat.templatesObj = {
        t_kvov: jsformat.getSpanClass('kvov'),
        t_exp: jsformat.getSpanClass('e'),
        t_key: jsformat.getSpanClass('k'),
        t_string: jsformat.getSpanClass('s'),
        t_number: jsformat.getSpanClass('n'),

        t_null: jsformat.getSpanBoth('null', 'nl'),
        t_true: jsformat.getSpanBoth('true','bl'),
        t_false: jsformat.getSpanBoth('false','bl'),

        t_oBrace: jsformat.getSpanBoth('{','b'),
        t_cBrace: jsformat.getSpanBoth('}','b'),
        t_oBracket: jsformat.getSpanBoth('[','b'),
        t_cBracket: jsformat.getSpanBoth(']','b'),

        t_ellipsis: jsformat.getSpanClass('ell'),
        t_blockInner: jsformat.getSpanClass('blockInner'),

        t_colonAndSpace: document.createTextNode(':\u00A0'),
        t_commaText: document.createTextNode(','),
        t_dblqText: document.createTextNode('"')
    };

    jsformat.generalClick = function(ev) {
        if (ev.which === 1) {
            var elem = ev.target;
            if (elem.className === 'e') {
                ev.preventDefault();
                var parent = elem.parentNode,
                    scrollTop = document.body.scrollTop
                  ;
                // Expand or collapse
                if (parent.classList.contains('collapsed')) {
                    // EXPAND
                    if (ev.ctrlKey)
                        jsformat.expand(parent.parentNode.children);
                    else
                        jsformat.expand([parent]);
                }
                else {
                    // COLLAPSE
                    if (ev.ctrlKey)
                        jsformat.collapse(parent.parentNode.children);
                    else
                        jsformat.collapse([parent]);
                }
                if (document.body.offsetHeight < window.innerHeight) {
                    return;
                }
                if (document.body.scrollTop === scrollTop) {
                    return;
                }
                document.body.scrollTop = scrollTop;
            }
        }
    };

    jsformat.jsonObjToHTML = function(obj, jsonpFunctionName) {
        var rootKvov = this.getKvovDOM(obj, false);
        rootKvov.classList.add('rootKvov');
        var divFormattedJson = document.createElement('DIV');
        divFormattedJson.id = 'formattedJson';
        divFormattedJson.appendChild( rootKvov);
        var returnHTML = divFormattedJson.outerHTML;
        if (jsonpFunctionName !== null) {
            returnHTML = '<div id="jsonpOpener">' + jsonpFunctionName + ' ( </div>' +
                    returnHTML + '<div id="jsonpCloser">)</div>';
        }
        return returnHTML;
    };
}());