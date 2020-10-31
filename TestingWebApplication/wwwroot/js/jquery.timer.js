(function ($) {
    $.fn.timered = function () {
        var timerUpdateMethod = function (block, endTimestamp) {
            var nowTimestamp = new Date().getTime();
            var distance = endTimestamp - nowTimestamp;

            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            var $this = $(block);
            $this.html(days + "d " + hours + "h " + minutes + "m " + seconds + "s ");
        };

        return this.each(function () {
            var block = $(this);
            var countdownTimestamp = $.trim(block.text());
            setInterval(function () { timerUpdateMethod(block, countdownTimestamp) }, 1000);
        });
    };
}(jQuery));
