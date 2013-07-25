<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:import href="zdev-report-common.xslt"/>
 
  <xsl:param name="docroot" >elemmap</xsl:param>
  <xsl:param name="selfname" >zdev-getelementsmap-report</xsl:param>
  <xsl:param name="title">Отчет о соответствии элементов структуре кода</xsl:param>
 
  <xsl:template name="elemmap-stat-table">
    <xsl:param name="class"></xsl:param>
    <table class="data elemmap stats {$class}">
      <thead>
        <tr>
          <th>№</th>
          <th>Метки</th>
          <th>Путь</th>
          <th>Тип кода</th>
		  <th>Информация</th>
		  <th>Число ссылок</th>
          <th>Варианты тегов</th>  
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="item"  mode="elemmap-stats">
          <xsl:sort select="@Path"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="result/item" mode="elemmap-stats">
    <tr class="obsolete-{Doc/@IsObsolete} error-{Doc/@IsError} question-{Doc/@IsQuestion}">
      <td class="number">
        <xsl:number value="position()" />
      </td>
      <td class="marks">
        <xsl:apply-templates select="Doc" mode="marks"/>
      </td>
      <td class="code">
		<a href="#attr-detail-{@Path}">
			<xsl:value-of select="@Path"/>
		</a>
      </td>
	   <td class="type">
        
        <xsl:apply-templates select="@Type"/>
      </td>
	  
       
      <td class="doc">
        <xsl:apply-templates select="Doc" mode="in-cell-short"/>

      </td>
     
      <td class="refcount">
        <xsl:value-of select="@ReferenceCount"/>
      </td>
	  
	   <td class="tagnames hidden" onclick="$(this).toggleClass('hidden')">
			<xsl:if test="@TagNames">
			  <div class="tagnames-div">
			   <xsl:value-of select="@TagNames"/>
			  </div>
			  <div class="tagnames-subst">
				Щелкните для просмотра
			  </div>
		  </xsl:if>
        </td>
    </tr>
  </xsl:template>

  <xsl:template match="@Type">
    <span class="type-{.}">
    <xsl:value-of select="."/>
    </span>
  </xsl:template>
  
  
  <xsl:template match="result/item" mode="elemmap-detail">
    <h3>
      <a name="attr-detail-{@Path}"/> Элемент <xsl:value-of select="@Path"/> ( <xsl:value-of select="@Type"/>)
    </h3>
    <div>
      <a href="#toc">назад к оглавлению</a>
    </div>
	<br/>
	
    <xsl:apply-templates select="Doc" mode="in-text-full"/>
	<br/>
    <h4>Ссылки в исходном коде</h4>
	<xsl:call-template name="elemmap-ref-table"/>
  </xsl:template>
  
  
  <xsl:template name="elemmap-ref-table">
    <table class="data elemmap refs">
      <thead>
		<tr>
			<th>№</th>
			<th>Путь</th>
		</tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="References/item" mode="ref-table">
          <xsl:sort select="@File" order="descending" data-type="number"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>
  
  

  <xsl:template match="result" mode="body">
    <h2>
      <a name="toc"/>Каталог элементов кода
    </h2>
    <xsl:call-template name="elemmap-stat-table">
      <xsl:with-param name="class">param</xsl:with-param>
    </xsl:call-template>
    <h2>Характеристика отдельных элементов</h2>
    <p>Внимание: в характеристику включены только атрибуты с описанием отдельных значений</p>
    <xsl:apply-templates select="item" mode="elemmap-detail">
      <xsl:sort select="@Path"/>
    </xsl:apply-templates>
  </xsl:template>

</xsl:stylesheet>
