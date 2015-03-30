// @codekit-prepend "libs/jquery.1.11.1.js";

// @codekit-prepend "../bootstrap-3.3.0/js/transition.js";
// @codekit-prepend "../bootstrap-3.3.0/js/alert.js";
// @codekit-prepend "../bootstrap-3.3.0/js/button.js";
// @codekit-prepend "../bootstrap-3.3.0/js/carousel.js";
// @codekit-prepend "../bootstrap-3.3.0/js/collapse.js";
// @codekit-prepend "../bootstrap-3.3.0/js/dropdown.js";
// @codekit-prepend "../bootstrap-3.3.0/js/modal.js";
// @codekit-prepend "../bootstrap-3.3.0/js/tooltip.js";
// @codekit-prepend "../bootstrap-3.3.0/js/popover.js";
// @codekit-prepend "../bootstrap-3.3.0/js/scrollspy.js";
// @codekit-prepend "../bootstrap-3.3.0/js/tab.js";
// @codekit-prepend "../bootstrap-3.3.0/js/affix.js";

// @codekit-prepend "plugins/waypoints.js";
// @codekit-prepend "plugins/waypoints-sticky.js";

// @codekit-prepend "plugins/jquery.cycle2.min.js";
// @codekit-prepend "plugins/jquery.cycle2.swipe.min.js";


// a sensible, reliable way to test for IE <= 9
var ie = (function(){
 
    var undef,
        v = 3,
        div = document.createElement('div'),
        all = div.getElementsByTagName('i');
    
    while (
        div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->',
        all[0]
    );
    
    return v > 4 ? v : undef;   
}());

// get viewport size in a cross browser style and fashion
function getViewport() {

  var viewPortWidth;
  var viewPortHeight;

  // the more standards compliant browsers (mozilla/netscape/opera/IE7) use window.innerWidth and window.innerHeight
  if (typeof window.innerWidth != 'undefined') {
    viewPortWidth = window.innerWidth,
    viewPortHeight = window.innerHeight
  }

  // IE6 in standards compliant mode (i.e. with a valid doctype as the first line in the document)
  else if (typeof document.documentElement != 'undefined'
  && typeof document.documentElement.clientWidth !=
  'undefined' && document.documentElement.clientWidth != 0) {
    viewPortWidth = document.documentElement.clientWidth,
    viewPortHeight = document.documentElement.clientHeight
  }

  // older versions of IE
  else {
   viewPortWidth = document.getElementsByTagName('body')[0].clientWidth,
   viewPortHeight = document.getElementsByTagName('body')[0].clientHeight
  }
  return [viewPortWidth, viewPortHeight];
};

// Debounce function (from underscore js)
function debounce(func, wait, immediate) {
  var timeout;
  return function() {
    var context = this, args = arguments;
    var later = function() {
      timeout = null;
      if (!immediate) func.apply(context, args);
    };
    var callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) func.apply(context, args);
  };
};

// any js that needs resetting when the browser dimensions change
function win_resize() {
  var win_resized_width = getViewport()[0];
  var win_resized_height = getViewport()[1];

  if (win_resized_width < 780) {
    $('.nav-level-2-menu').css('width',win_resized_width - 14);
    if ($('#mask').length < 1 ){
      $('body').append('<div id="mask"/>')
    }
  }
};



$(document).ready(function() {



  // fire on domready
  win_resize();
  // and again on resize(but debounced)
  var debounced_win_resize = debounce(win_resize, 100);
  jQuery(window).resize(debounced_win_resize);


  // add some classes for css / ie issues
  if (ie < 10){
    $('html').addClass('lte-ie9 ie-' + ie);
  }


  // crude navigation highlighter for testing purposes  TODO - replace
  var _current_path = window.location.href.substring(window.location.href.lastIndexOf('/'));
  $('.navbar a').each(function(){
    var _this_href = $(this).attr("href");
    _current_path = _current_path.split("?")[0].split("#")[0];
    //console.log(_current_path + "  " + _this_href);
    if (_current_path ==  _this_href) {
      $(this).parent().addClass('is-page-current');
    }
    // } else { // else is a dilemma
    //   $('.dropdown.nav-level-1').addClass('is-active');
    // }
  });

  // a plugin to fix the 'to answer' and 'to vote' things on scroll
  $('.js-sticky').waypoint('sticky');

  
  $('.js-nav-level-2-menu-trigger').on('click', function(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    $(this).parent().toggleClass('is-active');
    $('.page-bd #mask').toggle();
    //$(this).closest('.Dilemma-text').toggleClass('is-active');
  });





  // get the original height from the css 
  var Dilemma_text_height = $('Dilemma-text'[0]).height();

  $('.Dilemma-text-toggle').on('click', function(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    var el = $(this).closest('.Dilemma-text');
    var curHeight = el.height();
    var autoHeight = el.css('height', 'auto').height();
    console.log('curHeight = ' + curHeight);
    console.log('autoHeight = ' + autoHeight);
  
    el.css('height', curHeight);

    // el.toggleClass('is-active');
    if (curHeight == autoHeight) { // the box is open
      console.log('open');
      el.
        height(autoHeight)
        .animate({height: Dilemma_text_height}, 200, 'easeOutExpo')
        .toggleClass('is-active')
      ;
    } else { // the box is closed
      console.log('closed');
      el
        .height(curHeight)
        .animate({height: autoHeight}, 800, 'easeOutExpo')
        .toggleClass('is-active')
      ;
    }

  });


  //////////// Bootstrap inits
  // /$('[data-toggle="tooltip"]').tooltip();

  // $('.js-dismissable-trigger').on('click', function() {
  //   $(this).closest('.js-dismissable').fadeOut();
  // }); 


  /*

    Voting / best answer button

  ======================================================*/

  window.vote_countdown = 0;
  $('.js-voting-button:not([disabled])').on('click', function() {

    var _this =  $(this);

    if (window.vote_countdown == 0) {

      window.vote_countdown = 1;

      window.countdownElement = _this.children('.is-inactive');
      var  seconds = 3,
          second = 0,
          interval;

      vote_countdown_stash = countdownElement.text();

      window.interval = setInterval(
        function() {
          window.countdownElement.text('Voting in ' + (seconds - second));
          if (second >= seconds) {
            _this.addClass('is-active');
            _current_vote_displayed = _this.closest('.Dilemma-actions').find('.number-bubble--number');
            _current_vote_number = parseInt(_current_vote_displayed.text());
            _current_vote_displayed.text(_current_vote_number + 1);
            // all buttons of this type can now be disabled
            $('.stream .js-voting-button').attr('disabled','disabled');
            clearInterval(window.interval);
            // replace with correct action
            alert('[user would never see this] — action launched');
          }
          second++;
        }, 
        1000
      );

    } else {

      clearInterval(window.interval);
      window.interval = null;
      window.vote_countdown = 0;
      window.countdownElement.text(vote_countdown_stash);
      // replace with correct action
      alert('[user would never see this] — action cancelled');

    }
  });
  
  
  $('.js-bookmark-button:not([disabled])').on('click', function() {
    $(this).toggleClass('is-active');
    alert('[user would never see this] — Bookmarking action');
  });



  /*

    Flag button

  ======================================================*/

  $('.js-flag-button:not([disabled])').on('click', function() {
    window.flagTriggered = $(this);
  });
  $('.js-flag-confirm:not([disabled])').on('click', function() {
    window.flagTriggered.addClass('is-active').attr('disabled','disabled');
    alert('[user would never see this] — Action for' + window.flagTriggered);
  });


  

  /*

    Position the sticky headers 

  ======================================================*/
  // var width_of_stream = $('.stream').width() - 16;
  // $('.js-sticky .js-sticky--offset').each(function() {
  //   $(this).css({
  //     'max-width': width_of_stream,
  //     'margin': '0 auto'
  //   });
  // });
  
  /*

    mobile menu show / hide

  ======================================================*/
  $('.nav-dilemmas-mobile-toggle').on('click', function(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    $('.nav-dilemmas-mobile').toggle();
  });


}); // $(document).ready(function()







/*
 * jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
 *
 * Uses the built in easing capabilities added In jQuery 1.1
 * to offer multiple easing options
 *
 * TERMS OF USE - jQuery Easing
 * 
 * Open source under the BSD License. 
 * 
 * Copyright Â© 2008 George McGinley Smith
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list 
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse 
 * or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE. 
 *
*/

// t: current time, b: begInnIng value, c: change In value, d: duration
jQuery.easing['jswing'] = jQuery.easing['swing'];

jQuery.extend( jQuery.easing,
{
  def: 'easeOutQuad',
  swing: function (x, t, b, c, d) {
    //alert(jQuery.easing.default);
    return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
  },
  easeInQuad: function (x, t, b, c, d) {
    return c*(t/=d)*t + b;
  },
  easeOutQuad: function (x, t, b, c, d) {
    return -c *(t/=d)*(t-2) + b;
  },
  easeInOutQuad: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t + b;
    return -c/2 * ((--t)*(t-2) - 1) + b;
  },
  easeInCubic: function (x, t, b, c, d) {
    return c*(t/=d)*t*t + b;
  },
  easeOutCubic: function (x, t, b, c, d) {
    return c*((t=t/d-1)*t*t + 1) + b;
  },
  easeInOutCubic: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t*t + b;
    return c/2*((t-=2)*t*t + 2) + b;
  },
  easeInQuart: function (x, t, b, c, d) {
    return c*(t/=d)*t*t*t + b;
  },
  easeOutQuart: function (x, t, b, c, d) {
    return -c * ((t=t/d-1)*t*t*t - 1) + b;
  },
  easeInOutQuart: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t*t*t + b;
    return -c/2 * ((t-=2)*t*t*t - 2) + b;
  },
  easeInQuint: function (x, t, b, c, d) {
    return c*(t/=d)*t*t*t*t + b;
  },
  easeOutQuint: function (x, t, b, c, d) {
    return c*((t=t/d-1)*t*t*t*t + 1) + b;
  },
  easeInOutQuint: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
    return c/2*((t-=2)*t*t*t*t + 2) + b;
  },
  easeInSine: function (x, t, b, c, d) {
    return -c * Math.cos(t/d * (Math.PI/2)) + c + b;
  },
  easeOutSine: function (x, t, b, c, d) {
    return c * Math.sin(t/d * (Math.PI/2)) + b;
  },
  easeInOutSine: function (x, t, b, c, d) {
    return -c/2 * (Math.cos(Math.PI*t/d) - 1) + b;
  },
  easeInExpo: function (x, t, b, c, d) {
    return (t==0) ? b : c * Math.pow(2, 10 * (t/d - 1)) + b;
  },
  easeOutExpo: function (x, t, b, c, d) {
    return (t==d) ? b+c : c * (-Math.pow(2, -10 * t/d) + 1) + b;
  },
  easeInOutExpo: function (x, t, b, c, d) {
    if (t==0) return b;
    if (t==d) return b+c;
    if ((t/=d/2) < 1) return c/2 * Math.pow(2, 10 * (t - 1)) + b;
    return c/2 * (-Math.pow(2, -10 * --t) + 2) + b;
  },
  easeInCirc: function (x, t, b, c, d) {
    return -c * (Math.sqrt(1 - (t/=d)*t) - 1) + b;
  },
  easeOutCirc: function (x, t, b, c, d) {
    return c * Math.sqrt(1 - (t=t/d-1)*t) + b;
  },
  easeInOutCirc: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return -c/2 * (Math.sqrt(1 - t*t) - 1) + b;
    return c/2 * (Math.sqrt(1 - (t-=2)*t) + 1) + b;
  },
  easeInElastic: function (x, t, b, c, d) {
    var s=1.70158;var p=0;var a=c;
    if (t==0) return b;  if ((t/=d)==1) return b+c;  if (!p) p=d*.3;
    if (a < Math.abs(c)) { a=c; var s=p/4; }
    else var s = p/(2*Math.PI) * Math.asin (c/a);
    return -(a*Math.pow(2,10*(t-=1)) * Math.sin( (t*d-s)*(2*Math.PI)/p )) + b;
  },
  easeOutElastic: function (x, t, b, c, d) {
    var s=1.70158;var p=0;var a=c;
    if (t==0) return b;  if ((t/=d)==1) return b+c;  if (!p) p=d*.3;
    if (a < Math.abs(c)) { a=c; var s=p/4; }
    else var s = p/(2*Math.PI) * Math.asin (c/a);
    return a*Math.pow(2,-10*t) * Math.sin( (t*d-s)*(2*Math.PI)/p ) + c + b;
  },
  easeInOutElastic: function (x, t, b, c, d) {
    var s=1.70158;var p=0;var a=c;
    if (t==0) return b;  if ((t/=d/2)==2) return b+c;  if (!p) p=d*(.3*1.5);
    if (a < Math.abs(c)) { a=c; var s=p/4; }
    else var s = p/(2*Math.PI) * Math.asin (c/a);
    if (t < 1) return -.5*(a*Math.pow(2,10*(t-=1)) * Math.sin( (t*d-s)*(2*Math.PI)/p )) + b;
    return a*Math.pow(2,-10*(t-=1)) * Math.sin( (t*d-s)*(2*Math.PI)/p )*.5 + c + b;
  },
  easeInBack: function (x, t, b, c, d, s) {
    if (s == undefined) s = 1.70158;
    return c*(t/=d)*t*((s+1)*t - s) + b;
  },
  easeOutBack: function (x, t, b, c, d, s) {
    if (s == undefined) s = 1.70158;
    return c*((t=t/d-1)*t*((s+1)*t + s) + 1) + b;
  },
  easeInOutBack: function (x, t, b, c, d, s) {
    if (s == undefined) s = 1.70158; 
    if ((t/=d/2) < 1) return c/2*(t*t*(((s*=(1.525))+1)*t - s)) + b;
    return c/2*((t-=2)*t*(((s*=(1.525))+1)*t + s) + 2) + b;
  },
  easeInBounce: function (x, t, b, c, d) {
    return c - jQuery.easing.easeOutBounce (x, d-t, 0, c, d) + b;
  },
  easeOutBounce: function (x, t, b, c, d) {
    if ((t/=d) < (1/2.75)) {
      return c*(7.5625*t*t) + b;
    } else if (t < (2/2.75)) {
      return c*(7.5625*(t-=(1.5/2.75))*t + .75) + b;
    } else if (t < (2.5/2.75)) {
      return c*(7.5625*(t-=(2.25/2.75))*t + .9375) + b;
    } else {
      return c*(7.5625*(t-=(2.625/2.75))*t + .984375) + b;
    }
  },
  easeInOutBounce: function (x, t, b, c, d) {
    if (t < d/2) return jQuery.easing.easeInBounce (x, t*2, 0, c, d) * .5 + b;
    return jQuery.easing.easeOutBounce (x, t*2-d, 0, c, d) * .5 + c*.5 + b;
  }
});

/*
 *
 * TERMS OF USE - EASING EQUATIONS
 * 
 * Open source under the BSD License. 
 * 
 * Copyright Â© 2001 Robert Penner
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list 
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse 
 * or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE. 
 *
 */