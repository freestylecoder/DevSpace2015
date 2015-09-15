<%@ Page Title="" Language="C#" MasterPageFile="~/DevSpace.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DevSpace.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
<style>
	input {
		width: 90%;
		margin: 10px 5%;
		height: 50px;
		display: block;
	}

	input[type=text], input[type=email] {
		text-indent: 5%;
		font-weight: 400;
		font-size: x-large;
	}

	label {
		color: #000000;
		margin: 0 5%;
		font-weight: 400;
		font-size: large;
		font-variant: small-caps;
		background-color: #FFFFFF;
	}

	input[type=submit] {
		border: none;
		color: #FFFFFF;
		font-weight: 600;
		font-size: xx-large;
		font-variant: small-caps;
		background-color: #3852A4;
	}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<div id="ContentHeader">
		<img id="Logo" src="images/DevSpaceLogo400Wide.gif" alt="DevSpace Logo" />
		<h1>North Alabama's Premier<br />
			Polyglot Technology Conference
		</h1>
		<h2>
			October 9th &amp; 10th<br />
			Von Braun Center
		</h2>
		<h3>
			700 Monroe Street<br />
			Huntsville, AL 35801
		</h3>
	</div>

	<article>
		<h1>Speakers and Sessions</h1>
		<p>
			The speaker and session lineups are now available.
			Check in the menu under the Agenda to get all the details.
		</p>
	</article>

	<article>
		<h1>The Call for Speakers has Ended</h1>
		<p>
			Our call for speakers is officially closed.
			We had a wonderful turnout, and there will be some tough decisions involved.
			We hope to notify our selected speakers by August 20.
			This will give them a few days to acknowledge the offer.
			Then, check back with us on August 24th to see the speaker and session lineup.
		</p>
		<p>
			Don't forget that the Early Bird ticket sales go away when the speakers are anounced.
			Register now and save!
		</p>
	</article>

	<article>
		<h1>Room Block Available</h1>
		<p>
			We are pleased to offer our room block with the Embassy Suites in downtown Huntsville.
			Details can be found on the <a href="Travel.aspx">Travel Page.</a>
		</p>
	</article>

	<article>
		<h1>Call for Speakers Extended</h1>
		<p>
			Due to bad scheduling on my part, the Call for Speakers is now extended until August 17th.
			Visit the <a href="Speakers.aspx">Speakers Page</a> to submit your session.
		</p>
	</article>

	<article>
		<h1>Keynote Speaker</h1>
		<p>
			We are extremely pleased to announce that Alan Stevens will be the keynote speaker for DevSpace.
			Alan is a talented and passionate speaker with a strong commitment to community.
			We are thrilled that he will share his passion with you.
		</p>
		<p>
			Not only will Alan deliver our keynote, he has agreed to facilitate our open spaces.
			For those not familiar with the open spaces concept, the open spaces are a dedicated area of the conference that allows people to start or continue conversations on topics of interest.
			Guided by the law of two feet, people are free to enter and participate in the conversation at their own will.
			Once they have contributed or received what they look for, they are free to leave at their own will.
			Alan will be there to help facilitate these conversation with whatever the group needs.

		</p>
	</article>

	<article>
		<h1>Call for Speakers Open</h1>
		<p>The call for speakers is officially open. It is scheduled to close on July 29th. Visit the speakers page to submit your session.</p>
	</article>

	<article>
		<h1>Call for Sponsors Open</h1>
		<p>The call for sponsors is officially open. Go to the sponsors page for more information.</p>
	</article>

	<article>
		<!-- Begin MailChimp Signup Form -->
		<div id="mc_embed_signup">
			<form action="//devspaceconf.us11.list-manage.com/subscribe/post?u=0ffd19f5fce8c576f0cb4bd65&amp;id=ebabe2f6ce" method="post" id="mc-embedded-subscribe-form" name="mc-embedded-subscribe-form" class="validate" target="_blank" novalidate>
				<div id="mc_embed_signup_scroll">
					<h1>Subscribe to our mailing list</h1>
					<div class="mc-field-group">
						<label for="mce-EMAIL">
							Email Address (Required)
						</label>
						<input type="email" value="" name="EMAIL" class="required email" id="mce-EMAIL" />
					</div>
					<div class="mc-field-group">
						<label for="mce-FNAME">First Name </label>
						<input type="text" value="" name="FNAME" class="" id="mce-FNAME" />
					</div>
					<div class="mc-field-group">
						<label for="mce-LNAME">Last Name </label>
						<input type="text" value="" name="LNAME" class="" id="mce-LNAME" />
					</div>
					<p style="float:right;">Powered by <a href="http://eepurl.com/bubCub" title="MailChimp - email marketing made easy and fun">MailChimp</a></p>
					<p><a href="http://us11.campaign-archive2.com/home/?u=0ffd19f5fce8c576f0cb4bd65&id=ebabe2f6ce" title="View previous campaigns">View previous campaigns.</a></p>
					<div id="mce-responses" class="clear">
						<div class="response" id="mce-error-response" style="display: none"></div>
						<div class="response" id="mce-success-response" style="display: none"></div>
					</div>
					<!-- real people should not fill this in and expect good things - do not remove this or risk form bot signups-->
					<div style="position: absolute; left: -5000px;">
						<input type="text" name="b_0ffd19f5fce8c576f0cb4bd65_ebabe2f6ce" tabindex="-1" value="" />
					</div>
					<div class="clear">
						<input type="submit" value="Subscribe" name="subscribe" id="mc-embedded-subscribe" class="button" />
					</div>
				</div>
			</form>
		</div>

		<!--End mc_embed_signup-->
	</article>

	<article>
		<h1>Greetings.</h1>
		<p>We're still getting things set up. Expect more updates soon.</p>
		<p>Feel free to address inquires to <a href="mailto:info@devspaceconf.com">info@devspaceconf.com</a></p>
	</article>
</asp:Content>
