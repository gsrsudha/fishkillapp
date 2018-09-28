/*function sendRequest() {
    console.log("In sendRequest() method call.");
    var data = document.getElementById("txtName").value;
    console.log("name =" + data);
    var xhr = new XMLHttpRequest();
    //xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        console.log(this.statusCode);
        if (this.readyState === 4) {
            console.log(this.responseText);
        }
    });
    //http://localhost:7071/
    //https://fishkillapp.azurewebsites.net/
    xhr.open("GET", "http://localhost:7071/api/ValidateFishKill?name=" + data, true);
    xhr.setRequestHeader("Cache-Control", "no-cache");
    //xhr.setRequestHeader("Content-Type", "application/form-data");

    xhr.send(data);
    
}*/

$(document).ready(function () {
    $("#btnSubmit").click(function () {
        //$.get("http://localhost:7071/api/ValidateFishKill?name=Sudha_Reddy", function (data) {
        //    alert("Data Loaded: " + data);
        //});
        $.ajax({
            url: "http://localhost:7071/api/ValidateFishKill?name=Sudha_Reddy",
            type: "GET",            
            //dataType: "xml",
            //processData: false,
            //contentType: "text/plain",
            error: function (result, status, err) {
                alert("error: ", result.responseText + "\nerror: ", status.responseText + "\nerror: ", err.Message);
            }, success: function (response) {
                alert("Success: " + response);
            }
        });        
    });
});