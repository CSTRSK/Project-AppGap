 <?php  
 $message = '';  
 $error = '';  

 if(isset($_POST["submit"]))  
 { 
 $data = array(
     'secret' => "No Secret GitHub",
     'response' => $_POST['h-captcha-response']
 );
$verify = curl_init();
curl_setopt($verify, CURLOPT_URL, "https://hcaptcha.com/siteverify");
curl_setopt($verify, CURLOPT_POST, true);
curl_setopt($verify, CURLOPT_POSTFIELDS, http_build_query($data));
curl_setopt($verify, CURLOPT_RETURNTRANSFER, true);
$response = curl_exec($verify);
// var_dump($response);
$responseData = json_decode($response);
if($responseData->success) {
// your success code goes here
if(empty($_POST["name"]))  
      {  
           $error = "<label class='text-danger'>App Name erforderlich!</label>";  
      }  
      else if(empty($_POST["date"]))  
      {  
           $error = "<label class='text-danger'>Datum erforderlich!</label>";  
      }  
      else if(empty($_POST["state"]))  
      {  
           $error = "<label class='text-danger'>Status erforderlich!</label>";  
      }   
      else if(empty($_POST["response"]))  
      {  
           $error = "<label class='text-danger'>Antwort erforderlich!</label>";  
      } 
      else  
      {  
           if(file_exists('test.json'))  
           {  
                $current_data = file_get_contents('test.json');  
                $array_data = json_decode($current_data, true);  
                $extra = array(  
                     'appname'=>$_POST['name'],  
                     'date'=>$_POST["date"],  
                     'state'=>$_POST["state"],
                     'response' =>$_POST["response"]  
                );  
				array_unshift($array_data, $extra);
				$array_data[] = $extra;
                $final_data = json_encode($array_data);  
                if(file_put_contents('test.json',$final_data))  
                {  
                     $message = "<label class='text-success'>Danke! Deine Rückmeldung wurde Gespeichert!</p>";
					 $_POST = array();
                }  
           }  
           else  
           {  
                $error = 'Datenbank fehler!';  
           }  
} 
}else {
     // return error to user; they did not pass
     $error = "<label class='text-danger'>Sie haben leider das Captcha falsch ausgefüllt. Bitte versuchen Sie es erneut.</label>";  
     
     }
}

 ?>  
<!DOCTYPE html>
<html> 
<head>
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title>Project AppGap - Rückmeldung Einreichen</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="description" content="Project AppGap ein Projekt von CSTRSK Entwicklungsstudio Ulm, die App und Software-Entwickler in Ulm, Deutschland" />
	<meta name="keywords" content="Entwicklungsstudio,Entwicklungs,Studio,App, Entwickler, Programmierer, Ulm, Windows, Phone, Nokia, Android, Amazon, Yandex, Shop, Store, App, Apps, Microsoft,Web, Seite,Windows Phone,Software,Developers,dev,Germany,deutschland,samsung,lg,huawei,google,development,Project, AppGap" />
	<meta name="author" content="CSTRSK" />
	<meta name="owner" content="CSTRSK" />
	<link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
	<script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
	<script type="text/javascript" src="//cstrsk.de/project-appgap/ruckmeldung/jquery-1.5.2.js"></script>
</head>


<body>
<div class="w3-container w3-indigo">
  <h1>Project AppGap - Rückmeldung Einreichen</h1>
  <br>
<div class="w3-bar w3-light-grey">
  <a href="https://cstrsk.de/" style="width:25%" class="w3-bar-item w3-button w3-mobile">CSTRSK Hauptseite</a>
  <a href="https://cstrsk.de/project-appgap/" style="width:25%" class="w3-bar-item w3-button w3-mobile">Project Seite</a>
  <a href="#" style="width:25%" class="w3-bar-item w3-button w3-mobile w3-indigo">Rückmeldung Eintragen</a>
  <a href="https://cstrsk.de/project-appgap/ruckmeldung/view.html" style="width:25%" class="w3-bar-item w3-button w3-mobile">Rückmeldungen Sehen</a>
</div>
</div>

<div class="w3-container w3-mobile">
<br>
<form class="w3-container w3-light-grey w3-mobile" method="post">
<br>
				<?php if($error===''){}else{echo '<span class="w3-tag w3-xlarge w3-padding w3-red">'.$error.'</span>';}?>  
<br>
<label>App Name</label>
<input class="w3-input w3-mobile" name="name" type="text" accept-charset="UTF-8">

<label>Datum</label>
<input class="w3-input w3-mobile" name="date" type="text" value="<?php echo date('d.m.Y');?>" accept-charset="UTF-8">

<label>Status</label>
<select class="w3-select" name="state">
  <option value="" disabled selected>Wähle den Status der App</option>
  <option value="available"> Verfügbar </option>
  <option value="inwork"> in Arbeit </option>
  <option value="checking"> Überprüfen </option>
  <option value="rejected"> Abgelehnt </option>
</select> 

<label>Antwort</label>
					 <textarea name="response" cols="40" rows="5" style="width: 100%;" accept-charset="UTF-8"></textarea>
					 <br>
                          <div class="h-captcha" data-sitekey="No Public Key GitHub"></div>
                          		<br>
		
		
				<?php if($message===''){}else{echo '<span class="w3-tag w3-xlarge w3-padding w3-green">'.$message.'</span>';}?>  
				<br>
     <input type="submit" name="submit" value="Rückmeldungen Einreichen" class="w3-button w3-indigo w3-mobile" /> 
 <br><br>

</form>



<center><iframe title='A-ADS' data-aa='905845' src='//ad.a-ads.com/905845?size=468x60' scrolling='no' style='width:468px; height:60px; border:0px; padding:0;overflow:hidden' allowtransparency='true'></iframe></center>
</div>

<div class="w3-container w3-indigo">
  <h5 class="w3-text-white">&copy; Copyright 2019 by <a href="https://cstrsk.de/" rel="noopener" target="_blank">CSTRSK</a> All rights reserved. - <a href="https://cstrsk.de/Impressum.html" rel="noopener" target="_blank">CSTRSK Impressum</a> - <a href="https://cstrsk.de/dsvgo.html" rel="noopener" target="_blank">DSVGO auf CSTRSK / GDPR on CSTRSK</a></h5>
</div>

<script src="https://js.hcaptcha.com/1/api.js" async defer></script>
</body>
</html>

