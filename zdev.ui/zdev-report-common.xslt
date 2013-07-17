<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
   <xsl:param name="selfname" >default</xsl:param>
  <xsl:param name="title">УКАЖИТЕ ИМЯ ОТЧЕТА</xsl:param>
    

  <xsl:template match="result">
    <html>
      <head>
        <title>
          <xsl:value-of select="$title"/>
        </title>
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
      
   <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
    
  <xsl:template name="attr-stat-table">
    <xsl:param name="class"></xsl:param>
    <table class="data attribute stats {$class}">
      <thead>
        <tr>
          <th>№</th>
          <th>Код атрибута</th>
          <th>Число значений</th>
          <th>Число ссылок</th>
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="item"  mode="attr-stats">
          <xsl:sort select="@Name"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="result/item" mode="attr-stats">
    <tr>
      <td class="number">
        <xsl:number value="position()" />
      </td>
      <td class="code">
        <xsl:choose>
          <xsl:when test="ValueVariants/item">
            <a href="#attr-detail-{@Name}">
              <xsl:value-of select="@Name"/>            
            </a>
          
          </xsl:when>
          <xsl:otherwise><xsl:value-of select="@Name"/></xsl:otherwise>
        </xsl:choose> 
      </td>
       <td class="varcount">
        <xsl:value-of select="@VariantCount"/>
      </td>
      <td class="refcount">
        <xsl:value-of select="@ReferenceCount"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="result/item" mode="attr-detail">
    <h3>
      <a name="attr-detail-{@Name}"/> Атрибут <xsl:value-of select="@Name"/>
    </h3>
    <div>
      <a href="#toc">назад к оглавлению</a>
    </div>
    <xsl:call-template name="attr-variant-stat-table"/>
  </xsl:template>

  <xsl:template name="attr-variant-stat-table">
    <table class="data attr-variant stats">
      <thead>
        <th>№</th>
        <th>Значение</th>
        <th>Число ссылок</th>
        <th>Ссылки</th>
      </thead>
      <tbody>
        <xsl:apply-templates select="ValueVariants/item" mode="attr-variant-stats">
          <xsl:sort select="@ReferenceCount" order="descending" data-type="number"/>
        </xsl:apply-templates>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="ValueVariants/item" mode="attr-variant-stats">
    <tr>
      <td class="number">
        <xsl:number value="position()" />
      </td>
      <td class="value">
        <xsl:value-of select="@Value"/>
      </td>
      <td class="refcount">
        <xsl:value-of select="@ReferenceCount"/>
      </td>
      <td class="refs">
        <xsl:apply-templates select="References" mode="ref-list"/>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="References" mode="ref-list">
    <ul class="attr-ref l0" >
      <xsl:apply-templates select="item" mode="ref-list">
        <xsl:with-param name="level" select="1"/>
      </xsl:apply-templates>
    </ul>
  </xsl:template>

  <xsl:template match="References//item" mode="ref-list">
    <xsl:param name="level"/>
    <li>
      <xsl:apply-templates select="." mode="ref-desc"/>
      <xsl:if test=".//item">
        <ul  class="l{$level}">
          <xsl:apply-templates select="./item" mode="ref-list" >
            <xsl:with-param name="level" select="$level + 1"/>
          </xsl:apply-templates>
        </ul>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="References//item" mode="ref-desc">
    <xsl:if test="@File">
      <xsl:value-of select="@File"/>
      <xsl:if test="@MainContext"> :: </xsl:if>
    </xsl:if>
    <xsl:if test="@MainContext">
      <xsl:value-of select="@MainContext"/>
      <xsl:if test="@SubContext"> :: </xsl:if>
    </xsl:if>
    <xsl:if test="@SubContext">
      <xsl:value-of select="@SubContext"/>
      <xsl:if test="@Line"> :: </xsl:if>
    </xsl:if>
    <xsl:if test="@Line">
      <xsl:value-of select="@Line"/>
    </xsl:if>
  </xsl:template>


  <xsl:template match="result" mode="body">
    <h2>
      <a name="toc"/> Общая статистика использования параметров
    </h2>
    <xsl:call-template name="attr-stat-table">
      <xsl:with-param name="class">param</xsl:with-param>
    </xsl:call-template>
    <h2>Характеристика отдельных атрибутов</h2>
    <p>Внимание: в характеристику включены только атрибуты с описанием отдельных значений</p>
    <xsl:apply-templates select="item[ValueVariants/item]" mode="attr-detail">
      <xsl:sort select="@Name"/>
    </xsl:apply-templates>
  </xsl:template>

</xsl:stylesheet>
