function AbrirVentana(theURL, winName, features, myWidth, myHeight, isCenter) {
    if (window.screen) if (isCenter) if (isCenter == "true") {
        var myW = (screen.width - (screen.width * (myWidth / 100)));
        var myH = (screen.height - (screen.height * (myHeight / 100)));
        var myW1 = screen.width - myW;
        var myH1 = screen.height - myH;
        var myLeft = myW / 2;
        var myTop = myH / 2;
        features += (features != '') ? ',' : '';
        features += ',left=' + myLeft + ',top=' + myTop;
    }
    window.open(theURL, winName, features + ((features != '') ? ',' : '') + 'width=' + myW1 + ',height=' + myH1);
}
function getValuesArray() {
    var TristateUseDefault = -2, TristateTrue = -1, TristateFalse = 0;

    var ForReading = 1, ForWriting = 2, ForAppending = 8;
    var fso, textStream;
    var value1, value2, value3;
    var i;
    var values = new Array();

    try {
        fso = new ActiveXObject("Scripting.FileSystemObject");
        textStream = fso.OpenTextFile("C:\\GateGourmet\\TrolleyWeightReader\\Data\\test.txt");
    }
    catch (err) {
        alert(err.message);
    }
    i = 0;
    //Lee contenido de archivo y cada valor queda en una pos del arreglo
    while (!(textStream.AtEndOfStream)) {
        values[i] = textStream.ReadLine();
        i += 1;
    }
    textStream.Close();
    return values
}


function getValue() {
    var values;
    var value, value1, value2, value3;

    values = getValuesArray();
    i = 0;
    while (values.length < 3) {
        values = getValuesArray();
        i += 1;
        if (i > 100) {
            return -1
            break;
        }
    }
    //recorre arreglo de valores
    for (i = 0; i < values.length; i++) {
        if ((values.length - i) == 3) {
            value1 = values[i];
        }
        if ((values.length - i) == 2) {
            value2 = values[i];
        }
        if ((values.length - i) == 1) {
            value3 = values[i];
        }
    }

    return value1;
    /*
    if((value1==value2) && (value1==value3)){
    return value1;      
    }
    else{
    return -1;
    }
    */
}

function setValue(controlID) {
    var value, i;
    i = 0;

    value = getValue();
    //        while (value <= 0) {
    //            value = getValue();
    //            if (i > 100) {
    //                alert("Error capturando peso.  Verifique e intente de nuevo.");
    //                return;
    //                break;
    //            }
    //            i += 1;
    //        }

    //alert(value);
    if (document.getElementById(controlID) != null) {
        document.getElementById(controlID).value = value;
    }

}