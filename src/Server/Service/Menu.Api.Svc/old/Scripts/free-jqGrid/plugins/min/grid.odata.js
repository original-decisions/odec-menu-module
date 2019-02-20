(function(e){e.jgrid.odataHelper={resolveJsonReferences:function(a,e){function d(b,a,n){if("object"!==typeof b||!b)return b;if("[object Array]"===Object.prototype.toString.call(b)){for(c=0;c<b.length;c++){if("object"!==typeof b[c]||!b[c])return b[c];b[c]=b[c].$ref?d(b[c],c,b):d(b[c],a,b)}return b}if(b.$ref){f=b.$ref;if(k[f])return k[f];e.push([n,a,f])}else{if(b.$id){a=b.$id;delete b.$id;if(b.$values)b=b.$values.map(d);else for(var h in b)b.hasOwnProperty(h)&&(b[h]=d(b[h],h,b));k[a]=b}return b}}var c,
f,k={};e=e||[];"string"===typeof a&&(a=JSON.parse(a));a=d(a);for(c=0;c<e.length;c++)f=e[c],f[0][f[1]]=k[f[2]];return a},convertXmlToJson:function(a){var g={},d,c,f,k;if(!a)return null;if(1===a.nodeType){if(0<a.attributes.length)for(g["@attributes"]={},d=0;d<a.attributes.length;d++)c=a.attributes.item(d),g["@attributes"][c.nodeName]=c.nodeValue}else 3===a.nodeType?g=a.nodeValue:a.nodeType||(g=a);if(a.hasChildNodes&&a.hasChildNodes())for(d=0;d<a.childNodes.length;d++){c=a.childNodes.item(d);if(3===
c.nodeType)return c.nodeValue;f=c.nodeName;void 0===g[f]?g[f]=e.jgrid.odataHelper.convertXmlToJson(c):(void 0===g[f].push&&(k=g[f],g[f]=[],g[f].push(k)),g[f].push(e.jgrid.odataHelper.convertXmlToJson(c)))}return e.isEmptyObject(g)?null:g},parseMetadata:function(a,g){function d(b){var a={},k=[],h={},d=e("Schema",b).attr("Namespace")+".";e("EntityContainer EntitySet",b).each(function(b,h){a[e(h).attr("EntityType").replace(d,"")]=e(h).attr("Name");k.push(e(h).attr("Name"))});e("EntityType, ComplexType",
b).each(function(){var d,l,c,f,g,r,q,p;l=e(this).find("Property,NavigationProperty");f=(c=e("Key PropertyRef",this))&&0<c.length?c.first().attr("Name"):"";c=e(this).attr("Name");l&&(d=[],l.each(function(h,a){p={};e.each(a.attributes,function(){p[this.name]=this.value});g=p.Name===f;q="NavigationProperty"===a.tagName;r="Property"===a.tagName&&0<e('ComplexType[Name="'+p.Name+'"]',b).length;d.push(e.extend({iskey:g,isComplex:r,isNavigation:q,isCollection:0<=e.inArray(p.Name,k)},p))}),a[c]&&(h[a[c]]=
d),h[c]=d)});return h}function c(b){var a,k,h,d,c,f,g,w,x,r,q,p={},u={},m=[];for(c=0;c<b.EntityContainer.Elements.length;c++)u[b.EntityContainer.Elements[c].Type.ElementType.Definition.Name]=b.EntityContainer.Elements[c].Name,m.push(b.EntityContainer.Elements[c].Name);for(c=0;c<b.SchemaElements.length;c++)if(k=Array.prototype.concat(b.SchemaElements[c].DeclaredProperties,b.SchemaElements[c].NavigationProperties).filter(function(b){return!!b}),h=(a=b.SchemaElements[c].DeclaredKey)&&0<a.length?a[0].Name:
"",q=b.SchemaElements[c].Name,k){a=[];for(f=0;f<k.length;f++)d=k[f].Name===h,x=k[f].Type.IsNullable,r=k[f].Type.Definition.Namespace+k[f].Type.Definition.Name,g=!!k[f].Type.Definition.DeclaredProperties,w=!g&&!k[f].Type,a.push({Name:k[f].Name,Type:r,Nullable:x,iskey:d,isComplex:g,isNavigation:w,isCollection:0<=e.inArray(k[f].Name,m)});u[q]&&(p[u[q]]=a);p[q]=a}return p}function f(b){var a,k,h,d,c,f,g,w,x,r,q={},p={},u=[],m=[],z=[],m=b.dataServices.schema[0],A=m.namespace+".";for(b=0;b<m.entityContainer[0].entitySet.length;b++)p[m.entityContainer[0].entitySet[b].entityType.replace(A,
"")]=m.entityContainer[0].entitySet[b].name,u.push(m.entityContainer[0].entitySet[b].name);for(b=0;b<m.complexType.length;b++)z.push(m.complexType[b].name);m=Array.prototype.concat(m.entityType,m.complexType).filter(function(b){return!!b});for(b=0;b<m.length;b++)if(k=Array.prototype.concat(m[b].property,m[b].navigationProperty).filter(function(b){return!!b}),h=(a=m[b].key)&&0<a.propertyRef.length?a.propertyRef[0].name:"",r=m[b].name,k){a=[];for(c=0;c<k.length;c++)d=k[c].name===h,w="false"!==k[c].nullable,
x=k[c].type,f=!!k[c].type&&0<=e.inArray(k[c].type.replace(A,""),z),g=!k[c].type,a.push({Name:k[c].name,Type:x,Nullable:w,iskey:d,isComplex:f,isNavigation:g,isCollection:0<=e.inArray(k[c].name,u)});p[r]&&(q[p[r]]=a);q[r]=a}return q}var k;switch(g){case "xml":k=d(a);break;case "json":k=c(a);break;case "datajs":k=f(a)}return k},loadError:function(a,g,d){var c=a.status,f=d;if(!a.responseJSON)if(a.responseXML)a.responseText=a.responseText.replace(/<(\/?)([^:>\s]*:)?([^>]+)>/g,"<$1$3>"),a.responseXML=e.parseXML(a.responseText),
a.responseJSON=e.jgrid.odataHelper.convertXmlToJson(a.responseXML);else if(a.responseText)try{a.responseJSON=e.parseJSON(a.responseText)}catch(k){}if(a.responseJSON){if(a=a.responseJSON["@odata.error"]||a.responseJSON["odata.error"]||a.responseJSON.error)a.innererror?a.innererror.internalexception?(g=a.innererror.internalexception.message,f=a.innererror.internalexception.stacktrace||""):(g=a.innererror.message,f=a.innererror.stacktrace||""):(g=a.message.value||a.message,f=a.stacktrace||"")}else d&&
e.isPlainObject(d)&&(g=d.message,f=d.stack,c=d.code);return"<div>Status/error code: "+c+"</div><div>Message: "+g+'</div><div style="font-size: 0.8em;">'+f+"</div><br/>"}};e.jgrid.cmTemplate.odataComplexType={editable:!1,formatter:function(a,g,d){return e(this).jqGrid("odataJson",a,g,d)}};e.jgrid.cmTemplate.odataNavigationProperty={editable:!1,formatter:function(a,g,d){if(!g.colModel.odata.expand||"link"===g.colModel.odata.expand)return e(this).jqGrid("odataLink",a,g,d);if("json"===g.colModel.odata.expand)return e(this).jqGrid("odataJson",
a,g,d);if("subgrid"===g.colModel.odata.expand)return e(this).jqGrid("odataSubgrid",a,g,d)}};e.jgrid.cmTemplate["Edm.GeographyPoint"]={editable:!1,formatter:function(a,g,d){a||"xml"!==this.p.datatype||(a=e(d).filter(function(){return this.localName.toLowerCase()===g.colModel.name.toLowerCase()}),a=e.jgrid.odataHelper.convertXmlToJson(a[0]));return a.crs&&a.coordinates?e.jgrid.format("<div>{0}</div><div>[{1},{2}]</div>",a.crs.properties.name,a.coordinates[0],a.coordinates[1]):e.jgrid.format("<div>{0}</div>",
a)}};e.jgrid.extend({odataLink:function(a,g,d){var c=this[0].p;if("xml"!==c.datatype){if(d[g.colModel.name+"@odata.navigationLink"])return a=d[g.colModel.name+"@odata.navigationLink"],g=e.jgrid.format('<a href="{0}/{1}" target="_self">{2}</a>',c.odata.baseUrl,a,g.colModel.name);a=d[c.jsonReader.id]}else a=function(a){return e(d).filter(function(){return this.localName&&this.localName.toLowerCase()===a}).text()}(c.xmlReader.id.toLowerCase());return g=c.odata.iscollection?e.jgrid.format('<a href="{0}({1})/{2}" target="_self">{2}</a>',
c.url,a,g.colModel.name):e.jgrid.format('<a href="{0}/{1}" target="_self">{1}</a>',c.url,g.colModel.name)},odataJson:function(a,g,d){var c,f={};"xml"===this[0].p.datatype&&(a=e(d).filter(function(){return this.localName.toLowerCase()===g.colModel.name.toLowerCase()}),a=e.jgrid.odataHelper.convertXmlToJson(a[0]));for(c in a)a.hasOwnProperty(c)&&c&&0>c.indexOf("@odata.")&&0>c.indexOf("@attributes")&&(f[c]=a[c]);return JSON.stringify(f,null,1)},odataSubgrid:function(a,g,d){var c,f=this[0].p;a="xml"!==
f.datatype?d[f.jsonReader.id]:function(a){return e(d).filter(function(){return this.localName&&this.localName.toLowerCase()===a}).text()}(f.xmlReader.id.toLowerCase());for(c in f._index)if(f._index.hasOwnProperty(c)&&c&&a===f._index[c].toString()){a=c;break}return g=e.jgrid.format('<a style="cursor:pointer" data-id="{0}" onclick=\'$("#{2}").jqGrid("setGridParam", { odata: {activeEntitySet: "{1}" } });$("#{2}").jqGrid("expandSubGridRow", "{0}");return false;\'>{1}</a>',a,g.colModel.name,g.gid)},parseColumns:function(a,
g){for(var d=0,c,f,k,b,y,n=[],h,d=0;d<a.length;d++)c=0<="Edm.Int16,Edm.Int32,Edm.Int64".indexOf(a[d].Type),f=0<="Edm.Decimal,Edm.Double,Edm.Single".indexOf(a[d].Type),b=0<="Edm.Byte,Edm.SByte".indexOf(a[d].Type),k=a[d].Type&&0<=a[d].Type.indexOf("Edm.")&&(0<=a[d].Type.indexOf("Date")||0<=a[d].Type.indexOf("Time")),y=e.jgrid.cmTemplate[a[d].Type]?a[d].Type:a[d].isComplex?"odataComplexType":a[d].isNavigation?"odataNavigationProperty":c?"integerStr":f?"numberStr":b?"booleanCheckbox":"text",h={integer:c,
number:f,date:k,required:!a[d].Nullable||"false"===a[d].Nullable},c=c?"integer":f?"number":k?"datetime":b?"checkbox":"text",f=a[d].isNavigation||a[d].isComplex?'<span class="ui-icon ui-icon-arrowreturn-1-s" style="display:inline-block;vertical-align:middle;"></span>'+a[d].Name:a[d].Name,n.push(e.extend({label:f,name:a[d].Name,index:a[d].Name,editable:!a[d].isNavigation&&!a[d].iskey,searchrules:h,editrules:h,searchtype:c,inputtype:c,edittype:c,key:a[d].iskey,odata:{expand:a[d].isNavigation?g:a[d].isComplex?
"json":null,isnavigation:a[d].isNavigation,iscomplex:a[d].isComplex,iscollection:a[d].isCollection}},e.jgrid.cmTemplate[y]));return n},odataInit:function(a){function g(a,b,c,d){var h,l;if(b&&(c||"nu"===d||"nn"===d)){if(c)for(h=0;h<a.colModel.length;h++)if(l=a.colModel[h],l.name===b){if(l.odata.nosearch||l.odata.unformat&&(b=e.isFunction(l.odata.unformat)?l.odata.unformat(b,c,d):l.odata.unformat,!b))return;l.searchrules&&(l.searchrules.integer||l.searchrules.number||l.searchrules.date)?l.searchrules&&
l.searchrules.date&&(c=(new Date(c)).toISOString()):c="'"+c+"'";break}switch(d){case "in":case "cn":return"indexof("+b+",tolower("+c+")) gt -1";case "ni":case "nc":return"indexof("+b+",tolower("+c+")) eq -1";case "bw":return"startswith("+b+","+c+") eq true";case "bn":return"startswith("+b+","+c+") eq false";case "ew":return"endswith("+b+","+c+") eq true";case "en":return"endswith("+b+","+c+") eq false";case "nu":return b+" eq null";case "nn":return b+" ne null";default:return b+" "+d+" "+c}}}function d(a,
b){var c,e,h="";if(a.groups&&a.groups.length){for(c=0;c<a.groups.length;c++)h+="("+d(a.groups[c],b)+")",c<a.groups.length-1&&(h+=" "+a.groupOp.toLowerCase()+" ");a.rules&&a.rules.length&&(h+=" "+a.groupOp.toLowerCase()+" ")}if(a.rules.length)for(c=0;c<a.rules.length;c++)e=a.rules[c],(e=g(b,e.field,e.data,e.op))&&(h+=e+" "+a.groupOp.toLowerCase()+" ");return h=h.trim().replace(/\s(and|or)$/,"").trim()}function c(a,b,c,d){var h=a.colModel.filter(function(b){return b.name===a.odata.activeEntitySet})[0];
d=a._index[d];d=a.odata.iscollection?e.jgrid.format("{0}({1})/{2}",a.url,d,a.odata.activeEntitySet):e.jgrid.format("{0}/{1}",a.url,a.odata.activeEntitySet);var l={datatype:b.datatype,version:b.version,gencolumns:!1,expandable:b.expandable,odataurl:d,errorfunc:b.errorfunc,annotations:b.annotations,entitySet:a.odata.activeEntitySet};e("#"+c).html('<table id="'+c+'_t" class="scroll"></table>');e("#"+c+"_t").jqGrid({colModel:a.odata.subgridCols[a.odata.activeEntitySet],odata:e.extend({},a.odata,h.odata),
loadonce:!0,beforeInitGrid:function(){e(this).jqGrid("odataInit",l)},loadError:function(a,b,h){a=e.jgrid.odataHelper.loadError(a,b,h);a=e("#errdialog").html()+a;e("#errdialog").html(a).dialog("open")}})}function f(a,b){var f;f={datatype:b.datatype,jsonpCallback:b.callback};var n=function(h,d){return c(a,b,h,d)};a.odata||(a.odata={iscollection:!0});e.extend(a,{serializeGridData:function(h){var c=h;h={};a.odata.iscollection?(h={$top:c.rows,$skip:(parseInt(c.page,10)-1)*a.rowNum},"jsonp"===b.datatype&&
(h.$callback=b.callback),!b.version||4>b.version?(h.$inlinecount="allpages",h.$format="xml"===b.datatype?"atom":"application/json;odata=fullmetadata"):(h.$count=!0,h.$format="xml"===b.datatype?"atom":"application/json;odata.metadata=full"),c.sidx&&(h.$orderby=c.sidx+" "+c.sord),c._search&&(c.filters?(c=e.parseJSON(c.filters),c=d(c,a),0<c.length&&(h.$filter=c)):h.$filter=g(a,c.searchField,c.searchString,c.searchOper))):h.$format=!b.version||4>b.version?"xml"===b.datatype?"atom":"application/json;odata=fullmetadata":
"xml"===b.datatype?"atom":"application/json;odata.metadata=full";return this.p.odata.postData=h},ajaxGridOptions:f,mtype:"GET",url:b.odataurl},f);if(a.colModel)for(f=0;f<a.colModel.length;f++)if(a.colModel[f].odata&&"subgrid"===a.colModel[f].odata.expand){a.subGrid=!0;a.subGridRowExpanded=n;a.odata.activeEntitySet=a.colModel[f].name;a.loadonce=!0;break}n={contentType:"application/"+("jsonp"===b.datatype?"json":b.datatype)+";charset=utf-8",datatype:"jsonp"===b.datatype?"json":b.datatype};a.inlineEditing=
e.extend(!0,{beforeSaveRow:function(a,c){"edit"===a.extraparam.oper?(a.url=b.odataurl,a.mtype=b.odataverbs.inlineEditingEdit,a.url+="("+c+")"):(a.url=b.odataurl,a.mtype=b.odataverbs.inlineEditingAdd);return!0},serializeSaveData:function(a){return JSON.stringify(a)},ajaxSaveOptions:n},a.inlineEditing||{});e.extend(a.formEditing,{onclickSubmit:function(c,d,e){"add"===e?(c.url=b.odataurl,c.mtype=b.odataverbs.formEditingAdd):"edit"===e&&(c.url=b.odataurl+"("+d[a.id+"_id"]+")",c.mtype=b.odataverbs.formEditingEdit);
return d},ajaxEditOptions:n,serializeEditData:function(a){return JSON.stringify(a)}});e.extend(a.formDeleting,{url:b.odataurl,mtype:"DELETE",serializeDelData:function(){return""},onclickSubmit:function(a,b){a.url+="("+b+")";return""},ajaxDelOptions:n});n=(n=a.colModel.filter(function(a){return!!a.key})[0])?n.name:a.sortname||"id";"xml"===b.datatype?(b.annotations&&e.extend(!0,a,{loadBeforeSend:function(a){a.setRequestHeader("Prefer",'odata.include-annotations="*"')}}),e.extend(!0,a,{xmlReader:{root:function(b){b=
b.childNodes[0];b.innerHTML=b.innerHTML.replace(/<(\/?)([^:>\s]*:)?([^>]+)>/g,"<$1$3>");var c=e(b).attr("m:context");c&&(a.odata.baseUrl=c.substring(0,c.indexOf("/$metadata")),a.odata.entityType=c.substring(c.indexOf("#")+1).replace("/$entity",""));if(c=e(b).attr("m:type"))a.odata.entityType=c.replace("#","");return b},row:function(a){return a="entry"===a.localName?[a]:e(">entry",a)},cell:function(a){return e(">content>properties",a).get(0).childNodes},records:function(a){return e(">feed>entry",a).length},
page:function(){return Math.ceil((a.odata.postData.$skip+a.rowNum)/a.rowNum)},total:function(b){b=e(">feed>entry",b).length;return Math.ceil((a.odata.postData.$skip+a.rowNum)/a.rowNum)+(0<b?1:0)},repeatitems:!0,userdata:"userdata",id:n}})):(e.extend(!0,a,{jsonReader:{root:function(b){var c=b["@odata.context"];c&&(a.odata.baseUrl=c.substring(0,c.indexOf("/$metadata")),a.odata.entityType=c.substring(c.indexOf("#")+1).replace("/$entity",""));if(c=b["@odata.type"])a.odata.entityType=c.replace("#","");
return b.value||[b]},repeatitems:!0,id:n}}),b.annotations?e.extend(!0,a,{loadBeforeSend:function(a){a.setRequestHeader("Prefer",'odata.include-annotations="*"')},jsonReader:{records:function(a){return a[b.annotationName].records},page:function(a){return a[b.annotationName].page},total:function(a){return a[b.annotationName].total},userdata:function(a){return a[b.annotationName].userdata}}}):e.extend(!0,a,{jsonReader:{records:function(a){return a["odata.count"]||a["@odata.count"]},page:function(b){var c;
b["odata.nextLink"]?c=parseInt(b["odata.nextLink"].split("skip=")[1],10):(c=a.odata.postData.$skip+a.rowNum,b=b["odata.count"]||b["@odata.count"],c>b&&(c=b));return Math.ceil(c/a.rowNum)},total:function(b){return Math.ceil((b["odata.count"]||b["@odata.count"])/a.rowNum)},userdata:"userdata"}}))}return this.each(function(){var c=this,b=e(this),d=this.p;if(c.grid&&d){var g=e.extend(!0,{gencolumns:!1,odataurl:d.url,datatype:"json",entitySet:null,annotations:!1,annotationName:"@jqgrid.GridModelAnnotate",
odataverbs:{inlineEditingAdd:"POST",inlineEditingEdit:"PATCH",formEditingAdd:"POST",formEditingEdit:"PUT"}},a||{});"jsonp"===g.datatype&&(g.callback="jsonpCallback");if(g.entitySet){if(g.gencolumns){var h=e.extend(!0,{parsecolfunc:null,parsemetadatafunc:null,successfunc:null,errorfunc:null,async:!1,entitySet:null,metadatatype:a.datatype||"xml",metadataurl:(a.odataurl||d.url)+"/$metadata"},a||{});h.async&&(h.successfunc=function(){c.grid.hDiv&&(c.grid.hDiv.loading=!1);b.jqGrid("setGridParam",{datatype:g.datatype}).trigger("reloadGrid")},
c.grid.hDiv&&(c.grid.hDiv.loading=!0));b.jqGrid("odataGenColModel",h)}f(d,g)}else e.isFunction(g.errorfunc)&&g.errorfunc({},"entitySet cannot be empty",0)}})},odataGenColModel:function(a){var g=this[0],d=g.p,c=e(g),f,k,b=e.extend(!0,{parsecolfunc:null,parsemetadatafunc:null,successfunc:null,errorfunc:null,entitySet:null,metadataurl:d.url+"/$metadata",metadatatype:"xml",expandable:"link",async:!1},a||{});"jsonp"===b.metadatatype&&(b.callback="jsonpCallback");if(b.entitySet)return e.ajax({url:b.metadataurl,
type:"GET",dataType:b.metadatatype,jsonpCallback:b.callback,async:b.async,cache:!1}).done(function(a,g,h){var l=0,v=0,t=0;if("json"===b.metadatatype||"jsonp"===b.metadatatype)a=e.jgrid.odataHelper.resolveJsonReferences(a);f=c.triggerHandler("jqGridODataParseMetadata",a);!f&&e.isFunction(b.parsemetadatafunc)&&(f=b.parsemetadatafunc(a,g,h));if(f)k=f;else if(f=e.jgrid.odataHelper.parseMetadata(a,b.metadatatype))if(k=c.triggerHandler("jqGridODataParseColumns",[b,f]),!k&&e.isFunction(b.parsecolfunc)&&
(k=b.parsecolfunc(b,f)),!k)for(l in k={},f)f.hasOwnProperty(l)&&l&&(k[l]=c.jqGrid("parseColumns",f[l],b.expandable));if(k){for(t in k)if(k.hasOwnProperty(t)&&t)for(l=0;l<d.colModel.length;l++)for(v=0;v<k[t].length;v++)if(k[t][v].name===d.colModel[l].name){e.extend(!0,k[t][v],d.colModel[l]);break}d.colModel=k[b.entitySet];d.colModel||e.isFunction(b.errorfunc)&&b.errorfunc({data:a,status:g,xhr:h},"EntitySet "+b.entitySet+" is not found");d.odata||(d.odata={iscollection:!0});d.odata.subgridCols=k;e.isFunction(b.successfunc)&&
b.successfunc()}else e.isFunction(b.errorfunc)&&b.errorfunc({data:a,status:g,xhr:h},"parse $metadata error")}).fail(function(a,c,d){if(e.isFunction(b.errorfunc)){var f=e.jgrid.odataHelper.loadError(a,c,d);b.errorfunc({xhr:a,error:c,code:d},f)}}),k;e.isFunction(b.errorfunc)&&b.errorfunc({},"entitySet cannot be empty",0)}})})(jQuery);
