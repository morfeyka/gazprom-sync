/*
* @Copyright (c) 2011 John DeVight
* Permission is hereby granted, free of charge, to any person
* obtaining a copy of this software and associated documentation
* files (the "Software"), to deal in the Software without
* restriction, including without limitation the rights to use,
* copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following
* conditions:
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
* OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
* HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
* WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.
*/

/// <summary>
/// Constructor for the MessageBroker class.  Create an instance of an ArrayList
/// as the container for all the subscribers.
/// </summary>
function MessageBroker() {
    this.subscribers = [];
}

/// <summary>
/// Notify all subscribers who are interested in an event.
/// </summary>
/// <param name="event" type="string">Name of the event to notify subscribers about.</param>
/// <param name="args" type="object">arguments to be passed to the subscribers.</param>
MessageBroker.prototype.Notify = function(event, args) {
    var count = this.subscribers.length;

    // Loop through all the subscribers...
    for (var i = 0; i < count; i++) {
        // If the subscriber is for this event....
        if (this.subscribers[i].Event == event) {
            // Call the function pointer and pass in the arguements.
            this.subscribers[i].Subscriber(args);
        }
    }
}

/// <summary>
/// Add a subscriber for an event.
/// </summary>
/// <param name="event" type="string">Name of the event that the subscriber is interested in.</param>
/// <param name="subsctiber" type="function pointer">Pointer to a function to be called if the event occurs.</param>
MessageBroker.prototype.AddSubscriber = function(event, subscriber) {
    this.subscribers.push( { Event: event, Subscriber: subscriber });
}

/// <summary>
/// Remove a subscriber from an event.
/// </summary>
/// <param name="event" type="string">Name of the event that the subscriber was interested in.</param>
/// <param name="subsctiber" type="function pointer">Pointer to a function that was to be called if the event occurred.</param>
MessageBroker.prototype.RemoveSubscriber = function(event, subscriber) {
    JSON.removeArrayElement(this.subscribers, 'Subscriber', subscriber);
}

/// <summary>
/// Allow an object to inherit from another object.
/// </summary>
/// <param name="base" type="object">Base object to be inherited from.</param>
/// <param name="derived" type="object">Derived object to inherit from the base object.</param>
function inherits(base, derived) {
    for (var property in base) {
        try {
            derived[property] = base[property];
        }
        catch (warning) { }
    }
}

var _messageBroker = new MessageBroker();