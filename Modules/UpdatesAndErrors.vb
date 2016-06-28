Imports System.IO
Imports System.Net
Imports System.Net.Mail

Module UpdatesAndErrors

    ''' <summary>
    ''' The handler method for an unhandled exception. Sends info about the exception in an email to my email address.
    ''' </summary>
    Public Sub MyApplication_UnhandledException(
        ByVal sender As Object,
        ByVal e As ApplicationServices.UnhandledExceptionEventArgs
    )

        Dim Smtp_Server As New SmtpClient
        Dim e_mail As New MailMessage()
        Smtp_Server.UseDefaultCredentials = False
        Smtp_Server.Credentials = New Net.NetworkCredential("jamesmiddleton1@me.com", "J4m3s-M!")
        Smtp_Server.Port = 587
        Smtp_Server.EnableSsl = True
        Smtp_Server.Host = "smtp.mail.me.com"

        e_mail = New MailMessage()
        e_mail.From = New MailAddress("jamesmiddleton1@me.com")
        e_mail.To.Add("jamesmiddleton1@me.com")
        e_mail.Subject = "Unhandled Exception in G-Sim"
        e_mail.IsBodyHtml = False
        e_mail.Body = "The following exception occured when being used by " & Environment.UserName & ":" + vbCrLf + e.Exception.ToString()

        Try
            Smtp_Server.Send(e_mail)
        Catch ex As Exception
            MsgBox("An unhandled exception just occured but as you are not connected to the internet, the details have not been sent to the developer.")
            MsgBox("The application will now close to prevent any harm.")
            e.ExitApplication = True
            Return
        End Try

        MsgBox("A message has been sent to the developer containing details of an error which just occurred. This bug will be fixed as soon as possible.")
        MsgBox("The application will now close to prevent any harm.")
        e.ExitApplication = True

    End Sub

    ''' <summary>
    ''' Checks if the downloaded application is the newest available. If not then prompts the user to download the new version.
    ''' </summary>
    Public Sub CheckVersion()

        Dim RecentVersion As Version
        Dim wResponse As WebResponse

        'Catch the connection to server error in the try block
        Try
            wResponse = WebRequest.Create("http://jamesmiddleton.esy.es/G-Sim/versioncheck.php").GetResponse()
        Catch ex As WebException
            Debug.Print("No internet connection")
            Return
        End Try

        Dim sr As New StreamReader(wResponse.GetResponseStream())
        Dim page As String = sr.ReadToEnd()

        Dim browser As New WebBrowser()
        browser.DocumentText = page
        browser.Document.OpenNew(True)
        browser.Document.Write(page)
        browser.Refresh()

        RecentVersion = Version.Parse(browser.Document.GetElementById("version").InnerText)

        Dim CurrentVersion As Version = My.Application.Info.Version

        If RecentVersion > CurrentVersion Then
            If MsgBox(
"There is a more up-to-date version of this application. 
This version is " & CurrentVersion.ToString & ".
The newer version is " & RecentVersion.ToString & ".

Click Ok to go to download the new application.", MsgBoxStyle.OkCancel, "New Update") = MsgBoxResult.Ok Then
                Process.Start("http://jamesmiddleton.esy.es/G-Sim/download.php")
            End If
        End If

    End Sub

End Module
