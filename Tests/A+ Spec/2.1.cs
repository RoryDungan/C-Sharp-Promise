﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Promise.Tests.A__Spec
{
    public class _2_1
    {
		[Fact]
		public void _2_1_1_1_When_pending_a_promise_may_transition_to_either_the_fulfilled_or_rejected_state()
        {
            var pendingPromise1 = new Promise<object>();
            Assert.Equal(PromiseState.Pending, pendingPromise1.CurState);
            pendingPromise1.Resolve(new object());
            Assert.Equal(PromiseState.Resolved, pendingPromise1.CurState);
        
            var pendingPromise2 = new Promise<object>();
            Assert.Equal(PromiseState.Pending, pendingPromise2.CurState);
            pendingPromise2.Reject(new Exception());
            Assert.Equal(PromiseState.Rejected, pendingPromise2.CurState);
        }

		[Fact]
		public void _2_1_2_1_When_fulfilled_a_promise_must_not_transition_to_any_other_state()
        {
            var fulfilledPromise = new Promise<object>();
            fulfilledPromise.Resolve(new object());

            Assert.Throws<ApplicationException>(() => fulfilledPromise.Reject(new Exception()));

            Assert.Equal(PromiseState.Resolved, fulfilledPromise.CurState);
		}

        [Fact]
        public void _2_1_2_1_When_fulfilled_a_promise_must_have_a_value_which_must_not_change()
        {
            var promisedValue = new object();
            var fulfilledPromise = new Promise<object>();
            var handled = 0;

            fulfilledPromise.Then(v => 
            {
				Assert.Equal(promisedValue, v);
				++handled;
			});

            fulfilledPromise.Resolve(promisedValue);

            Assert.Throws<ApplicationException>(() => fulfilledPromise.Resolve(new object()));

            Assert.Equal(1, handled);
        }

		[Fact]
		public void _2_1_3_1_When_rejected_a_promise_must_not_transition_to_any_other_state()
        {
            var rejectedPromise = new Promise<object>();
            rejectedPromise.Reject(new Exception());

            Assert.Throws<ApplicationException>(() => rejectedPromise.Resolve(new object()));

            Assert.Equal(PromiseState.Rejected, rejectedPromise.CurState);
        }

		public void _2_1_3_2_When_rejected_a_promise_must_have_a_reason_which_must_not_change()
        {
            var rejectedPromise = new Promise<object>();
			var reason = new Exception();
            var handled = 0;

            rejectedPromise.Catch(e =>
            {
                Assert.Equal(reason, e);
                ++handled;
            });

            rejectedPromise.Reject(reason);

            Assert.Throws<ApplicationException>(() => rejectedPromise.Reject(new Exception()));

            Assert.Equal(1, handled);
		}			
    }
}
