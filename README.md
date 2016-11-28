[![Stories in Ready](https://badge.waffle.io/Puchaczov/CronExpression.png?label=ready&title=Ready)](https://waffle.io/Puchaczov/CronExpression)
# CronExpression
<h2 class="western" lang="en-US">What is CronExpression</h2>
<p lang="en-US">CronExpression is .NET CRON expression validator and fire time evaluator. It pretend to support the broadest spectrum of available syntax. It’s even powerful enough to point why you’re expression is wrong!</p>
<h2 class="western" lang="en-US">Latest release</h2>
<p lang="en-US">Version 3.0</p>
<ul>
	<li>
<p><span lang="en-US">Support mixable complex types such as L, LW, W, #, -(range), /(Hyphen), *(star) in defined by CRON rules segment</span></p>
	</li><li>
<p><span lang="en-US">Support expression in two modes: Modern(with second and year segment), Old(without second and year segment)</span></p>
	</li><li>
<p><span lang="en-US">Improved expression analysis. </span></p>
	</li><li>
<p><span lang="en-US">Support for nonstandard defintions</span></p>
</li><li>
<p><span lang="en-US">Nuget availibility (<b>Install-Package TQL.CronExpression</b>)</span></p>
</li></ul>
<h2 class="western" lang="en-US">Features</h2>
<p lang="en-US">Typing expression is allowed in two formats, standard and nonstandard.</p>
<p lang="en-US">Supported nonstandard definitions are:</p>

<ul>
	<li>
<p><span lang="en-US"><b>@annual</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@yearly</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@monthly</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@weekly</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@daily</b></span></p>
	</li><li>
<p><span lang="en-US"><b>@hourly</b></span></p>
</li></ul>
<p lang="en-US">Standard definitions are:</p>
<table cellpadding="0" cellspacing="0">
	<thead>
		<tr>
			<td style="border: 1.00pt solid #dddddd; padding: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Feature</font></font></p>
			</td>
			<td style="border-top: 1.00pt solid #dddddd; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0.05cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Parser</font></font></p>
			</td>
			<td style="border-top: 1.00pt solid #dddddd; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0.05cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Expression
				Validator</font></font></p>
			</td>
			<td style="border-top: 1.00pt solid #dddddd; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0.05cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Evaluator</font></font></p>
			</td>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">*</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">,</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">-</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">/</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">?</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth,DayOfWeek)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth,DayOfWeek)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">L</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">W</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">LW</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfMonth)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">#</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfWeek)</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported(DayOfWeek)</font></font></p>
			</td>
		</tr>
		<tr>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: 1.00pt solid #dddddd; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0.05cm; padding-right: 0.05cm" width="103">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Errors</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="104">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="172">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Supported</font></font></p>
			</td>
			<td style="border-top: none; border-bottom: 1.00pt solid #dddddd; border-left: none; border-right: 1.00pt solid #dddddd; padding-top: 0cm; padding-bottom: 0.05cm; padding-left: 0cm; padding-right: 0.05cm" width="224">
				<p><font color="#333333"><font style="font-size: 12pt" size="3">Not
				Applicable</font></font></p>
			</td>
		</tr>
	</tbody>
</table>
<p>&nbsp;</p>
<h2 class="western" lang="en-US">API &amp; Documentation</h2>
<p lang="en-US">To successfully use this library, you won’t need to learn a lot, just read short introduction in appropriate section of documentation (<b><a href="https://github.com/Puchaczov/TQL.CronExpression/wiki">go here</a></b>). If you would like to bring something new to library, open new issue</b>.</p>
<h2 class="western" lang="en-US">Contribute</h2>
<p lang="en-US">If you like this library, please
appreciate my effort. You can do it by creating fixes, new
features, reporting issues or starring library. Feel free to report ideas,
It's good place to consider implementation of it.</p>
<h2 class="western" lang="en-US">License</h2>
<p lang="en-US">This library is based on <b>MIT</b> license.</p>
