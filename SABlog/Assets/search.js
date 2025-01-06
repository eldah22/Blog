document.addEventListener('DOMContentLoaded', function () {
    $('input.datetimepicker').datepicker({
        duration: '',
        changeMonth: true,
        changeYear: true,
        changeTime: true,
        yearRange: '2000:2100',
        showTime: true,
        time24h: true,
        dateFormat: "d/m/yy",
        showOn: "both",
        buttonText: "<i class='fas fa-calendar-week'></i>"
    });

    $.datepicker.regional['sq'] = {
        closeText: 'Mbyll',
        prevText: 'Para',
        nextText: 'Pas',
        currentText: 'Zgjedhur',
        monthNames: ['Janar', 'Shkurt', 'Mars', 'Prill', 'Maj', 'Qershor', 'Korruk', 'Gusht',
            'Shtator', 'Tetor', 'Nentor', 'Dhjetor'
        ],
        monthNamesShort: ['Janar', 'Shkurt', 'Mars', 'Prill', 'Maj', 'Qershor', 'Korruk', 'Gusht',
            'Shtator', 'Tetor', 'Nentor', 'Dhjetor'
        ],
        dayNames: ['E Hene', 'E Marte', 'E Merkure', 'E Enjte', 'E Premte', 'E Shtune', 'E Diele'],
        dayNamesShort: ['he', 'ma', 'mer', 'enj', 'pre', 'sht', 'dje'],
        dayNamesMin: ['he', 'ma', 'mer', 'enj', 'pre', 'sht', 'dje'],
        weekHeader: 'Java',
        dateFormat: 'd/m/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: true,
        yearSuffix: ''
    };

    $.datepicker.setDefaults($.datepicker.regional['sq']);
});