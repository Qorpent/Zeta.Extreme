<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>

  <xsl:template match="/">
    <html>
      <head>
        <title>Отчет о соответствии настроек администраторов</title>
        <style>
          table {
          border-collapse:collapse;
          }
          table td, table th {
          border : solid 1px gray;
          }
          tr.v-true {
          background-color: #ffcc00;
          }
          tr.SlotList-true {
          font-weight: bold;
          }
        </style>
      </head>
      <body>
        <h1>Отчет о соответствии настроек администраторов</h1>
        <table>
          <thead>
            <th>Номер</th>
            <th>Статус</th>
            <th>Предприятие</th>
            <th>Администратор(ы)</th>
          </thead>
          <tbody>
            <xsl:apply-templates select="//root/result/item"/>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="item">
    <xsl:choose>
      <xsl:when test="@IsValid = 'true'">
        <xsl:choose>
          <xsl:when test="Admins/item/@SlotList!=''">
            <tr class="fa-{@FileAttached} cnt-{@ActiveAdminCount} v-{@IsValid} SlotList-true">
              <td>
                <xsl:value-of select="position()"/>
              </td>
              <td>
                <ul>
                  <xsl:if test="@IsValid = 'true'">
                    <li>ОК</li>
                  </xsl:if>
                  <xsl:if test="@FileAttached = 'false'">
                    <li>Нет файла</li>
                  </xsl:if>
                  <xsl:if test="@ActiveAdminCount = 0">
                    <li>Админ не назначен</li>
                  </xsl:if>
                  <xsl:if test="@ActiveAdminCount &gt; 1">
                    <li>Несколько администраторов</li>
                  </xsl:if>
                </ul>
              </td>
              <td>
                <xsl:value-of select="@ObjectName"/>
              </td>
              <td>
                <ul>
                  <xsl:apply-templates select="Admins/item"/>
                </ul>
              </td>
            </tr>
          </xsl:when>
          <xsl:otherwise>
            <tr class="fa-{@FileAttached} cnt-{@ActiveAdminCount} v-{@IsValid}">
              <td>
                <xsl:value-of select="position()"/>
              </td>
              <td>
                <ul>
                  <xsl:if test="@IsValid = 'true'">
                    <li>ОК</li>
                  </xsl:if>
                  <xsl:if test="@FileAttached = 'false'">
                    <li>Нет файла</li>
                  </xsl:if>
                  <xsl:if test="@ActiveAdminCount = 0">
                    <li>Админ не назначен</li>
                  </xsl:if>
                  <xsl:if test="@ActiveAdminCount &gt; 1">
                    <li>Несколько администраторов</li>
                  </xsl:if>
                </ul>
              </td>
              <td>
                <xsl:value-of select="@ObjectName"/>
              </td>
              <td>
                <ul>
                  <xsl:apply-templates select="Admins/item"/>
                </ul>
              </td>
            </tr>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="Admins/item/@SlotList!=''">
        <tr class="fa-{@FileAttached} cnt-{@ActiveAdminCount} v-{@IsValid} SlotList-true">
          <td>
            <xsl:value-of select="position()"/>
          </td>
          <td>
            <ul>
              <xsl:if test="@IsValid = 'true'">
                <li>ОК</li>
              </xsl:if>
              <xsl:if test="@FileAttached = 'false'">
                <li>Нет файла</li>
              </xsl:if>
              <xsl:if test="@ActiveAdminCount = 0">
                <li>Админ не назначен</li>
              </xsl:if>
              <xsl:if test="@ActiveAdminCount &gt; 1">
                <li>Несколько администраторов</li>
              </xsl:if>
            </ul>
          </td>
          <td>
            <xsl:value-of select="@ObjectName"/>
          </td>
          <td>
            <ul>
              <xsl:apply-templates select="Admins/item"/>
            </ul>
          </td>
        </tr>
          </xsl:when>
          <xsl:otherwise>
     <!---->
            <tr class="fa-{@FileAttached} cnt-{@ActiveAdminCount} v-{@IsValid}">
              <td>
                <xsl:value-of select="position()"/>
              </td>
              <td>
                <ul>
                  <xsl:if test="@IsValid = 'true'">
                    <li>ОК</li>
                  </xsl:if>
                  <xsl:if test="@FileAttached = 'false'">
                    <li>Нет файла</li>
                  </xsl:if>
                  <xsl:if test="@ActiveAdminCount = 0">
                    <li>Админ не назначен</li>
                  </xsl:if>
                  <xsl:if test="@ActiveAdminCount &gt; 1">
                    <li>Несколько администраторов</li>
                  </xsl:if>
                </ul>
              </td>
              <td>
                <xsl:value-of select="@ObjectName"/>
              </td>
              <td>
                <ul>
                  <xsl:apply-templates select="Admins/item"/>
                </ul>
              </td>
            </tr>
          </xsl:otherwise>
        </xsl:choose>
       
      </xsl:otherwise>
    </xsl:choose>
     </xsl:template>
  <xsl:template match="Admins/item">
    <li>
      (<xsl:value-of select="@Login"/>) <xsl:value-of select="@Name"/> ( <xsl:value-of select="@Occupation"/> ) 
      <xsl:if test="@SlotList!=''">
        (<xsl:value-of select="@SlotList"/>)
      </xsl:if>
    </li>
  </xsl:template>
</xsl:stylesheet>
