<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:import href="zdev-report-common.xslt"/>

  <xsl:param name="docroot" >elemmap</xsl:param>
  <xsl:param name="selfname" >zdev-parameters-report</xsl:param>
  <xsl:param name="title">Отчет о параметрах отчета</xsl:param>




  <xsl:template match="result" mode="body">
    <h2>
      <a name="toc"/>Каталог параметров
    </h2>
    <xsl:call-template name="param-list"/>

  </xsl:template>

  <xsl:template name="param-list" >
    <table class="data param-list">
      <thead>
        <tr>
          <th>№</th>
          <th>Код</th>
          <th>Название</th>
          <th>Тип определения</th>
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="item" mode="toc">
          <xsl:sort select="@Code"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="result/item" mode="toc">
    <tr class="param-toc">
      <td>
        <xsl:value-of select="position()"/>
      </td>
      <td>
        <xsl:value-of select="@Code"/>
        <xsl:if test=".//Source//@target">
          (<xsl:value-of select=".//Source//@target"/>)
        </xsl:if>
      </td>
      <td>
        <xsl:value-of select="@Name"/>
      </td>
      <xsl:variable name="haslib">
        <xsl:choose>
          <xsl:when test=".//Definitions//*[@CodeType='ParamDefLib']">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <td class="has-lib-{$haslib}">
        <xsl:apply-templates select=".//Definitions//@CodeType" mode="distinct">
          <xsl:with-param name="root" select="current()//Definitions" />
          <xsl:sort select="."/>
        </xsl:apply-templates>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="@CodeType" mode="after-distinct">
    <span class="range-{.}">
      <xsl:value-of select="."/>
    </span>
    <br/>
  </xsl:template>
  
  <xsl:template match="@*" mode="distinct">
    <xsl:param name="root" />
    <xsl:param name="custom-mode">default</xsl:param>
    <xsl:variable name="val" select="string(.)" />
    <xsl:variable name="attrname" select="local-name()"/>
    <xsl:if test="generate-id(.)=generate-id($root//@*[local-name()=$attrname and string(.)=$val])" >
      <xsl:apply-templates select="." mode="after-distinct" >
        <xsl:with-param name="custom-mode" select="$custom-mode"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
