# Don't change this file.
unless defined?(STACK_ROOT)
  root_path = File.join(File.dirname(__FILE__), '..')

  unless RUBY_PLATFORM =~ /mswin32/
    require 'pathname'
    root_path = Pathname.new(root_path).cleanpath(true).to_s
  end
  
  STACK_ROOT = File.expand_path(root_path)
end
 
unless defined?(Stack::Initializer)
  if File.directory?("#{STACK_ROOT}/tools/stack")
    $:.unshift(File.dirname(__FILE__) + "/../tools/stack/lib")
    require 'stack'
    require "#{STACK_ROOT}/tools/stack/lib/stack/vs"
  else
    require 'rubygems'
    Gem.manage_gems
    
    version = defined?(STACK_GEM_VERSION) ? STACK_GEM_VERSION : stack_gem_version
    stack_gem = Gem.cache.search('stack', "=#{version}").first

    if stack_gem
      gem 'stack', "=#{version}"
      require 'stack'
      require 'stack/vs'
    else
      STDERR.puts %(Cannot find gem for Stack =#{version}:
                    Install the missing gem with 'gem install -v=#{version} stack', or
                    change environment.rb to define STACK_GEM_VERSION with your desired version.)
      exit 1
    end
  end
end

def stack_gem_version
  environment_without_comments = IO.readlines(File.dirname(__FILE__) + '/environment.rb').reject { |l| l =~ /^#/ }.join
  environment_without_comments =~ /[^#]STACK_GEM_VERSION = '([\d.]+)'/
  $1
end
