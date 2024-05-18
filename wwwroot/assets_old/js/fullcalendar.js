//Full Calendar

var selectDate = '';

document.addEventListener('DOMContentLoaded', function () {
    //alert("addEventListener");
    $("#spnMonthName").html($(".fc-toolbar-title").html())
    var containerEl = document.getElementById('external-events');
    new FullCalendar.Draggable(containerEl, {
        itemSelector: '.fc-event',
        eventData: function (eventEl) {
            return {
                title: eventEl.innerText.trim(),
                title: eventEl.innerText,
                className: eventEl.className + ' overflow-hidden '
            }
        }
    });
    var calendarEl = document.getElementById('calendar2');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        timeZone: 'local',

        defaultView: 'month',


        navLinks: false, // can click day/week names to navigate views
        businessHours: true, // display business hours
        editable: true,
        selectable: true,
        selectMirror: true,
        droppable: true, // this allows things to be dropped onto the calendar
        select: function (arg) {



            //alert("select")
            $.ajax({
                //   url: directoryurl + '/Admin/Dashboard/GetDataTaskbyCustomDate',
                url: '/Admin/Dashboard/GetDataTaskbyCustomDate',
                data: 'TaskDate=' + convert(arg.start),
                success: function (data) {
                    $("#todoListPartialContainer").html(data);
                    /*calendar.unselect()*/

                    selectDate = arg.startStr;


                    Newcalload(arg.startStr);
                },
                error: function (error) {
                    alert("error");
                }
            });

            //var title = prompt('Event Title:');
            //if (title) {
            //    calendar.addEvent({
            //        title: title,
            //        start: arg.start,
            //        end: arg.end,
            //        allDay: arg.allDay
            //    })
            //}
            //calendar.unselect()
        },
        //eventClick: function(arg) {
        //    if (confirm('Are you sure you want to delete this event?')) {
        //        arg.event.remove()
        //    }
        //},
        //editable: true,
        //dayMaxEvents: true, // allow "more" link when too many events
        events: { url: "GetEventList" }, eventDidMount: function (info) {
            if (info.event.extendedProps.background) {
                info.el.style.background = info.event.extendedProps.background;
            }
        },
        eventRender: function (event, element, view) {
            element.find('.fc-title').each(function () {
                $(this).insertBefore($(this).prev('.fc-time'));
            });
        }
    });

    calendar.render();

    if (selectDate !== '') {
        $('.fc-day-future').removeClass('fc-day-future');
        $('td[data-date="' + selectDate + '"]').addClass('fc-day-select');
    }

});
function Newcalload(date) {

    //debugger
    //var containerEl = document.getElementById('external-events');
    //new FullCalendar.Draggable(containerEl, {
    //    itemSelector: '.fc-event',
    //    eventData: function (eventEl) {
    //        return {
    //            title: eventEl.innerText.trim(),
    //            title: eventEl.innerText,
    //            className: eventEl.className + ' overflow-hidden '
    //        }
    //    }
    //});
    var calendarEl = document.getElementById('calendar2');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },

        defaultView: 'month',
        initialDate: date,

        navLinks: false, // can click day/week names to navigate views
        businessHours: true, // display business hours
        editable: true,
        selectable: true,
        selectMirror: true,
        droppable: true, // this allows things to be dropped onto the calendar
        select: function (arg) {



            //alert("select")
            $.ajax({
                // url: directoryurl + '/Admin/Dashboard/GetDataTaskbyCustomDate',
                url: '/Admin/Dashboard/GetDataTaskbyCustomDate',
                data: 'TaskDate=' + convert(arg.start),
                success: function (data) {
                    $("#todoListPartialContainer").html(data);

                    selectDate = arg.startStr;

                    /*calendar.unselect()*/
                    Newcalload(arg.startStr);
                },
                error: function (error) {
                    alert("error");
                }
            });
            //var title = prompt('Event Title:');
            //if (title) {
            //    calendar.addEvent({
            //        title: title,
            //        start: arg.start,
            //        end: arg.end,
            //        allDay: arg.allDay
            //    })
            //}
            //calendar.unselect()
        },
        //eventClick: function (arg) {
        //    if (confirm('Are you sure you want to delete this event?')) {
        //        arg.event.remove()
        //    }
        //},
        //editable: true,
        //dayMaxEvents: true, // allow "more" link when too many events
        //events: [{
        //    title: 'Business Lunch',
        //    start: '2023-10-12',
        //    constraint: 'businessHours'
        //}, {
        //    title: 'Meeting',
        //    start: '2021-03-13T11:00:00',
        //    constraint: 'availableForMeeting', // defined below
        //    color: '#f35e90'
        //}, {
        //    title: 'Conference',
        //    start: '2021-07-18',
        //    end: '2021-07-20',
        //    color: '#e67e22'
        //}, {
        //    title: 'Party',
        //    start: '2021-07-29T20:00:00',
        //    color: '#22c865'
        //},
        //// areas where "Meeting" must be dropped
        //{
        //    id: 'availableForMeeting',
        //    start: '2021-03-11T10:00:00',
        //    end: '2021-03-11T16:00:00',
        //    rendering: 'background',
        //    color: '#5e72e4'
        //}, {
        //    id: 'availableForMeeting',
        //    start: '2021-03-13T10:00:00',
        //    end: '2021-03-13T16:00:00',
        //    rendering: 'background'
        //},
        //// red areas where no events can be dropped
        //{
        //    start: '2021-03-24',
        //    end: '2021-03-28',
        //    overlap: false,
        //    rendering: 'background',
        //    color: 'rgba(0,0,0,0.1)'
        //}, {
        //    start: '2021-03-06',
        //    end: '2021-03-11',
        //    overlap: false,
        //    rendering: 'background',
        //    color: 'rgba(0,0,0,0.1)'
        //}
        //]
        events: { url: "GetEventList" },
        eventDidMount: function (info) {
            if (info.event.extendedProps.background) {
                info.el.style.background = info.event.extendedProps.background;
            }
        },
        eventRender: function (event, element, view) {
            element.find('.fc-title').each(function () {
                $(this).insertBefore($(this).prev('.fc-time'));
            });
        }
    });

    calendar.render();

    if (selectDate !== '') {
        $('.fc-day-future').removeClass('fc-day-future');
        $('td[data-date="' + selectDate + '"]').addClass('fc-day-select');
    }
}

function convert(str) {
    var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [mnth, day, date.getFullYear()].join("-");
}


//List FullCalendar
