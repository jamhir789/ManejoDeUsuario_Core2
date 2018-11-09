
$('#modalEditar').on('shown.bs.modal', function () {
    $('#myInput').focus()
})


//toma la funcion mostrar usuario, al hacer click en el boton editar se ejecuta esta funcion
function getUsuario(id,action)
{
    $.ajax({
        type: "POST",
        url: action,
        data: {id},
        success: function (response) {
            mostrarUsuario(response);
        }

    });

}

//la variaable items obtiene todos los datos de abajo
var items;
///variables globales por cada propiedad del usuario
var id;
var userName;
var email;
var phoneNumber;
//otras variables donde almacenaremos los datos de registro, pero estos datos no seran modificados
var accessFailedCount;
var concurrencyStamp;
var emailConfirmed ;
var lockoutEnabled;
var lockoutEnd;
var normalizedUserName;
var normalizedEmail;
var passwordHash;
var phoneNumberConfirmed;
var securityStamp;
var twoFactorEnabled;




//funcion para mostrar loos datos
function mostrarUsuario(response)
{
    items = response;
    for (var i; i < 3;i++)
    {
        var x = document.getElementById('Select');
        x.remove(i);
    }

    $.each(items, function (index, val) {
        $('input[name=Id]').val(val.id);
        $('input[name=UserName]').val(val.userName);
        $('input[name=Email]').val(val.email);
        $('input[name=PhoneNumber]').val(val.phoneNumber);

    });

}

function editarUsuario(action)
{
    id = $('input[name=Id]')[0].value;
    email = $('input[name=Email]')[0].value;
    phoneNumber = $('input[name=PhoneNumber]')[0].value;

    $.each(items, function (index, val) {
        accessFailedCount = val.accessFailedCount;
        concurrencyStamp = val.concurrencyStamp;
        emailConfirmed = val.emailConfirmed;
        lockoutEnabled = val.lockoutEnabled;
        lockoutEnd = val.lockoutEnd;
        userName = val.userName;
        normalizedUserName = val.normalizedUserName;
        normalizedEmail = val.normalizedEmail;
        passwordHash = val.passwordHash;
        phoneNumberConfirmed = val.phoneNumberConfirmed;
        securityStamp = val.securityStamp;
        twoFactorEnabled = val.twoFactorEnabled;
    });
    
    $.ajax({
        type:"POST",
        url: action,
        data: {id,userName, email, phoneNumber, accessFailedCount, concurrencyStamp, emailConfirmed,
            lockoutEnabled, lockoutEnd,  normalizedEmail,normalizedUserName, 
            passwordHash, phoneNumberConfirmed, securityStamp, twoFactorEnabled },
        success: function (response)
        {
           
            if (response == "Save") {
                window.location.href = "Usuarios";
            }
            else
            {
                alert("No se puede editar los datos del usuario");
            }
        }


    });

}