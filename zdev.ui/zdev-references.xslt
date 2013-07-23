<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:template match="References" mode ="collapsed-cell">
    <xsl:choose>
      <xsl:when test="count(item) &gt; 4">
        <td class="refs hidden" onclick="$(this).toggleClass('hidden')">
          <div class="ref-div">
            <xsl:apply-templates select="." mode="ref-list"/>
          </div>
          <div class="ref-subst">
            Щелкните для просмотра
          </div>
        </td>
      </xsl:when>
      <xsl:otherwise>
        <td class="refs">
          <xsl:apply-templates select="." mode="ref-list"/>
        </td>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="References" mode="ref-list">
    <ul class="attr-ref l0" >
      <xsl:apply-templates select="item" mode="ref-list">
        <xsl:with-param name="level" select="1"/>
      </xsl:apply-templates>
    </ul>
  </xsl:template>
  
    <xsl:template match="References//item" mode="ref-table">
		<tr>
			<td><xsl:value-of select="position()"/></td>
			
			 <xsl:choose>
			  <xsl:when test="count(.//item) &gt; 4">
				<td class="refs hidden" onclick="$(this).toggleClass('hidden')">
				  <div class="ref-div">
					<xsl:apply-templates select="." mode="ref-list"/>
				  </div>
				  <div class="ref-subst">
					Щелкните для просмотра
				  </div>
				</td>
			  </xsl:when>
			  <xsl:otherwise>
				<td class="refs">
				  <xsl:apply-templates select="." mode="ref-list"/>
				</td>
			  </xsl:otherwise>
			</xsl:choose>
			
			
		
		</tr>
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



</xsl:stylesheet>
