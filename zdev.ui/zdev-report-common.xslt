<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:import href="zdev-references.xslt"/>
  <xsl:import href="zdev-doc.xslt"/>

  <xsl:param name="selfname" >default</xsl:param>
  <xsl:param name="docroot" >attr</xsl:param>
  <xsl:param name="title">УКАЖИТЕ ИМЯ ОТЧЕТА</xsl:param>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="result">
    <html>
      <head>
        <title>
          <xsl:value-of select="$title"/>
        </title>
        <script src="../scripts/jquery.min.js" type="text/javascript">&#160;</script>
        <meta generator="qorpent:xslt-render(${selfname}.xslt)" />
        <link rel="stylesheet" href="../styles/{$selfname}.css" />
      </head>
      <body>
        <div class="header">Разработчик Zeta v0.2</div>
        <h1>
          <xsl:value-of select="$title"/>
        </h1>
        <xsl:apply-templates select="." mode="body"/>
      </body>
    </html>

  </xsl:template>


</xsl:stylesheet>
