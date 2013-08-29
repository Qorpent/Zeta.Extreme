<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:template match="/">
    <html>
      <head>
        <title>Метрики кода</title>
        <script src="../scripts/jquery.min.js" type="text/javascript">&#160;</script>
        <meta generator="qorpent:zdev-metrics-report.xslt)" />
        <link rel="stylesheet" href="../styles/zdev-metrics-report.css" />
      </head>
      <body class="metrics">
        <header>
          <h1>Метрики кода</h1>
        </header>
        <section>
          <h2>Общие файловые метрики</h2>
          <article>
            <h3>В целом по репозиторию</h3>
            <xsl:call-template name="file-metrics" >
            <xsl:with-param name="area"></xsl:with-param>
            </xsl:call-template>
          </article>
          <article>
            <h3>Системные</h3>
            <xsl:call-template name="file-metrics" >
            <xsl:with-param name="area">sys_</xsl:with-param>
            </xsl:call-template>
          </article>
          <article>
            <h3>Пользовательские</h3>
            <xsl:call-template name="file-metrics" >
            <xsl:with-param name="area">usr_</xsl:with-param>
            </xsl:call-template>
          </article>
        </section>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="file-metrics">
    <xsl:param name="area"/>
    <table class="data file-metrics">
      <thead>
        <th>Показатель</th>
        <th>Всего</th>
        <th>%</th>
        <th>Среднее</th>
        <th>Минимум</th>
        <th>Максимум</th>
      </thead>
      <tbody>
        <tr>
          <td>Кол-во файлов</td>
          <td>
            <xsl:value-of select="//item[@SubGroup=$area and @ItemName='filecount' and @Type='']/@Value"/>
          </td>
          <td colspan="4"/>
        </tr>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Размер'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'size'"/>
        </xsl:call-template>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Кол-во символов'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'symbols'"/>       
        </xsl:call-template>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Кол-во строк'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'lines'"/>
        </xsl:call-template>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Код'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'codelines'"/>
        </xsl:call-template>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Комментарии'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'commentedlines'"/>
        </xsl:call-template>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Пустых строк'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'emptylines'"/>
        </xsl:call-template>
        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Закоментировано кода'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'commentedcodelines'"/>
        </xsl:call-template>

        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Элементов'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'elements'"/>
        </xsl:call-template>

        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Атрибутов'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'attributes'"/>
        </xsl:call-template>

        <xsl:call-template name="file-metric-row">
          <xsl:with-param name="title" select="'Текстовых элементов'"/>
          <xsl:with-param name="area" select="$area"/>
          <xsl:with-param name="item" select="'texts'"/>
        </xsl:call-template>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template name="file-metric-row">
    <xsl:param name="title"/>
    <xsl:param name="item"/>
    <xsl:param name="area"/>
    <tr>
      <td>
        <xsl:value-of select="$title"/>
      </td>
      <td>
        <xsl:value-of select="//item[@SubGroup=$area and @ItemName=$item and @Type='total']/@Value"/>
      </td>
      <td>
        <xsl:value-of select="//item[@SubGroup=$area and @ItemName=$item and @Type='perc']/@Value"/>
      </td>
      <td>
        <xsl:value-of select="//item[@SubGroup=$area and @ItemName=$item and @Type='avg']/@Value"/>
      </td>
      <td>
        <xsl:value-of select="//item[@SubGroup=$area and @ItemName=$item and @Type='min']/@Value"/>
      </td>
      <td>
        <xsl:value-of select="//item[@SubGroup=$area and @ItemName=$item and @Type='max']/@Value"/>
      </td>

    </tr>
  </xsl:template>
  

</xsl:stylesheet>
