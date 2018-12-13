function renderOblastInfo(oblastName, oblastId) {
    $.getJSON('/json/oblastsInfo.json', function (data) {
        for (let key in data) {
            let oblast = data[key];

            if (oblast.Name === oblastName) {
                $('#oblastName').html(oblast.Name);
                $('#oblastLandArea').html(oblast.LandArea);
                $('#oblastPopulation').html(oblast.Population);
                $('#oblastPopulationDensity').html(oblast.PopulationDensity);
                $('#oblastMunicipalitiesCount').html(oblast.MunicipalitiesCount);
                $('#oblastDescription').html(oblast.Description);
                $('#viewMonuments').attr('href', '/Monuments/AllForOblast?oblastId=' + oblastId);

                $('#viewMonuments').removeClass('disabled');
                $(window).scrollTop(0);                
            }
        }
    })
}